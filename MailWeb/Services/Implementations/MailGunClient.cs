using System;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MailManager.Models.Interfaces;
using MailManager.Services.Interfaces;
using MailWeb.Models.Interfaces;
using MailWeb.Models.MailHosts;

namespace MailWeb.Services.Implementations
{
    public class MailGunClient : IMailClient
    {
        #region Constructor

        public MailGunClient(IMailClientSetting mailGunClientSetting, IHttpClientFactory httpClientFactory)
        {
            _mailClientSetting = mailGunClientSetting;

            // Get mail gun host info.
            var mailGunHost = (MailGunHost) mailGunClientSetting.MailHost;

            _httpClient = httpClientFactory.CreateClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(mailGunClientSetting.Timeout);

            _httpClient.BaseAddress =
                new Uri("https://api.mailgun.net", UriKind.Absolute);

            var byteArray = Encoding.ASCII.GetBytes($"api:{mailGunHost.ApiKey}");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        #endregion

        #region Properties

        public string UniqueName => "MailGun";

        public string DisplayName => "MailGun";

        private readonly IMailClientSetting _mailClientSetting;

        private readonly HttpClient _httpClient;

        #endregion

        #region Methods

        public virtual async Task SendMailAsync(IMailAddress sender, IMailAddress[] recipients,
            IMailAddress[] carbonCopies,
            IMailAddress[] blindCarbonCopies, string subject, string content, bool isHtmlContent = false,
            ExpandoObject additionalSubjectData = null, ExpandoObject additionalContentData = null,
            CancellationToken cancellationToken = default)
        {
            var data = new MultipartFormDataContent($"www-{DateTime.Now:yy-MMM-dd}");
            data.Add(new StringContent(ToMailGunAddress(sender), Encoding.UTF8), "from");

            data.Add(new StringContent(string.Join(",", recipients.Select(ToMailGunAddress)), Encoding.UTF8), "to");

            var clonedCarbonCopies = (carbonCopies ?? new IMailAddress[0])
                .Concat(_mailClientSetting.CarbonCopies ?? new IMailAddress[0])
                .ToArray();

            if (clonedCarbonCopies.Length > 0)
                data.Add(
                    new StringContent(string.Join(",", clonedCarbonCopies.Select(ToMailGunAddress)), Encoding.UTF8),
                    "cc");

            var clonedBlindCarbonCopies = (blindCarbonCopies ?? new IMailAddress[0])
                .Concat(_mailClientSetting.BlindCarbonCopies ?? new IMailAddress[0])
                .ToArray();

            if (clonedBlindCarbonCopies.Length > 0)
                data.Add(
                    new StringContent(string.Join(",", clonedBlindCarbonCopies.Select(ToMailGunAddress)),
                        Encoding.UTF8),
                    "bcc");

            data.Add(new StringContent(await RenderAsync(subject, additionalSubjectData)), "subject");
            data.Add(new StringContent(await RenderAsync(content, additionalContentData)),
                isHtmlContent ? "html" : "text");

            await _httpClient
                .PostAsync(new Uri($"v3/{_mailGunClientSetting.Domain}/messages", UriKind.Relative), data,
                    cancellationToken);
        }

        public virtual async Task SendMailAsync(IMailAddress sender, IMailAddress[] recipients,
            IMailAddress[] carbonCopies,
            IMailAddress[] blindCarbonCopies, string templateName, ExpandoObject additionalSubjectData = null,
            ExpandoObject additionalContentData = null, CancellationToken cancellationToken = default)
        {
            // Find mail content asynchronously.
            var mailContent = await GetMailContentAsync(templateName, cancellationToken);
            if (mailContent == null)
                throw new Exception($"Template named {templateName} is not found.");

            await SendMailAsync(sender, recipients, carbonCopies, blindCarbonCopies, mailContent.Subject,
                mailContent.Content, mailContent.IsHtml,
                additionalSubjectData, additionalContentData, cancellationToken);
        }

        public virtual async Task SendMailAsync(string sender, IMailAddress[] recipients, IMailAddress[] carbonCopies,
            IMailAddress[] blindCarbonCopies, string templateName, ExpandoObject additionalSubjectData = null,
            ExpandoObject additionalContentData = null, CancellationToken cancellationToken = default)
        {
            // Find mail sender asynchronously.
            var mailSender = await GetSenderAsync(sender, cancellationToken);
            if (mailSender == null)
                throw new Exception($"Mail sender with key {sender} is not found.");

            // Find mail content asynchronously.
            var mailContent = await GetMailContentAsync(templateName, cancellationToken);
            if (mailContent == null)
                throw new Exception($"Template named {templateName} is not found.");

            await SendMailAsync(mailSender, recipients, carbonCopies, blindCarbonCopies, mailContent.Subject,
                mailContent.Content, mailContent.IsHtml,
                additionalSubjectData, additionalContentData, cancellationToken);
        }

        public virtual Task<IMailAddress> GetSenderAsync(string key, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IMailContent> GetMailContentAsync(string name,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Internal methods

        protected virtual string ToMailGunAddress(IMailAddress mailAddress)
        {
            return $"{mailAddress.DisplayName} <{mailAddress.Address}>";
        }

        protected virtual Task<string> RenderAsync(string initialContent, ExpandoObject additionalData)
        {
            return Task.FromResult(initialContent);
        }

        #endregion
    }
}