using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HttpMultipartParser;
using NetCoreTest.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace NetCoreTest.Interceptors
{
    public class MailGunHttpInterceptor : DelegatingHandler
    {
        #region Properties

        private readonly string _validSenderMailAddress;

        private readonly string _validRecipientMailAddress;

        #endregion

        #region Constructor

        public MailGunHttpInterceptor(string validSenderMailAddress, string validRecipientMailAddress)
        {
            _validSenderMailAddress = validSenderMailAddress;
            _validRecipientMailAddress = validRecipientMailAddress;
        }

        #endregion

        #region Methods

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            using (var stream = await request.Content.ReadAsStreamAsync())
            {
                var parameters = MultipartFormDataParser.Parse(stream);
                var from = parameters.Parameters.Where(x => x.Name == "from")
                    .Select(x => GetSingleMailAddress(x.Data))
                    .FirstOrDefault();

                var tos = parameters.Parameters.Where(x => x.Name == "to")
                    .Select(x => GetMultipleMailAddress(x.Data));

                var mailGunResponse = new MailGunResponse();

                if (from == _validSenderMailAddress && tos.Any(x => x.Contains(_validRecipientMailAddress)))
                {
                    mailGunResponse.Id = "<fake-domain.mailgun.org>";
                    mailGunResponse.Message = "Queued. Thank you.";
                }
                else
                {
                    mailGunResponse.Id = string.Empty;
                }

                var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
                httpResponseMessage.Content = new ReadOnlyMemoryContent(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(mailGunResponse)));
                return httpResponseMessage;
            }
        }

        protected virtual string GetSingleMailAddress(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            var lastSpaceIndex = value.LastIndexOf(" ", StringComparison.InvariantCultureIgnoreCase);
            var mail = value.Substring(lastSpaceIndex);

            return mail.Replace("<", "")
                .Replace(">", "")
                .Trim();
        }

        protected virtual List<string> GetMultipleMailAddress(string value)
        {
            return value
                .Split(",")
                .Select(GetSingleMailAddress)
                .ToList();
        }

        #endregion
    }
}