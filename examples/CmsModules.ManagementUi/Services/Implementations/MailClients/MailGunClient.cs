using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CmsModules.Shares.Models.MailHosts;
using MailModule.Models.Interfaces;
using MailModule.Services.Interfaces;

namespace CmsModules.ManagementUi.Services.Implementations.MailClients
{
    public class MailGunClient : IMailClient
    {
        #region Constructor

        public MailGunClient(IMailClientSetting mailGunClientSetting,
            IHttpClientFactory httpClientFactory)
        {
            _mailClientSetting = mailGunClientSetting;
            _mailGunHost = (MailGunHost) _mailClientSetting.MailHost;

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

        public string UniqueName => _mailClientSetting.UniqueName;

        public string DisplayName => _mailClientSetting.DisplayName;

        private readonly IMailClientSetting _mailClientSetting;

        private readonly MailGunHost _mailGunHost;

        private readonly HttpClient _httpClient;

        #endregion

        #region Events

        public delegate void OnMailSentEventHandler(HttpResponseMessage httpResponseMessage);

        public event OnMailSentEventHandler OnMailSent;

        #endregion

        #region Methods

        public virtual async Task SendMailAsync(IMailAddress sender, IMailAddress[] recipients,
            IMailAddress[] carbonCopies,
            IMailAddress[] blindCarbonCopies, string subject, string content, bool isHtmlContent = false,
            object additionalSubjectData = null, object additionalContentData = null,
            Attachment[] attachments = default,
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

            var httpResponseMessage = await _httpClient
                .PostAsync(new Uri($"v3/{_mailGunHost.Domain}/messages", UriKind.Relative), data,
                    cancellationToken);

            OnMailSent?.Invoke(httpResponseMessage);
        }

        public virtual async Task SendMailAsync(IMailAddress sender, IMailAddress[] recipients,
            IMailAddress[] carbonCopies,
            IMailAddress[] blindCarbonCopies, string templateName, object additionalSubjectData = null,
            object additionalContentData = null,
            Attachment[] attachments = default,
            CancellationToken cancellationToken = default)
        {
            // Find mail content asynchronously.
            var mailContent = await GetMailContentAsync(templateName, cancellationToken);
            if (mailContent == null)
                throw new Exception($"Template named {templateName} is not found.");

            await SendMailAsync(sender, recipients, carbonCopies, blindCarbonCopies, mailContent.Subject,
                mailContent.Content, mailContent.IsHtml,
                additionalSubjectData, additionalContentData,
                attachments,
                cancellationToken);
        }

        public virtual async Task SendMailAsync(string sender, IMailAddress[] recipients, IMailAddress[] carbonCopies,
            IMailAddress[] blindCarbonCopies, string templateName, object additionalSubjectData = null,
            object additionalContentData = null,
            Attachment[] attachments = default,
            CancellationToken cancellationToken = default)
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
                additionalSubjectData, additionalContentData, attachments, cancellationToken);
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

        protected virtual string ToMailGunAddress(IMailAddress mailAddress)
        {
            return $"{mailAddress.DisplayName} <{mailAddress.Address}>";
        }

        protected virtual Task<string> RenderAsync(string initialContent, object additionalData)
        {
            return Task.FromResult(initialContent);
        }


        #endregion
    }
}