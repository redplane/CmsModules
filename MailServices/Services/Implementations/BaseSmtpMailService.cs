using System;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using MailServices.Models.Interfaces;
using MailServices.Services.Interfaces;

namespace MailServices.Services.Implementations
{
    public class BaseSmtpMailService : IMailService
    {
        #region Constructor

        public BaseSmtpMailService(ISmtpMailServiceSetting smtpMailServiceSetting)
        {
            _smtpMailServiceSetting = smtpMailServiceSetting;
        }

        #endregion

        #region Properties

        public virtual string UniqueName => throw new NotImplementedException();

        public virtual string DisplayName => throw new NotImplementedException();

        private readonly ISmtpMailServiceSetting _smtpMailServiceSetting;

        #endregion

        #region Methods

        public virtual async Task SendMailAsync(IMailAddress sender, IMailAddress[] recipients,
            IMailAddress[] carbonCopies,
            IMailAddress[] blindCarbonCopies, string subject, string content, bool isHtmlContent = false,
            ExpandoObject additionalSubjectData = null, ExpandoObject additionalContentData = null,
            CancellationToken cancellationToken = default)
        {
            using (var smtpClient = GetSmtpClient(_smtpMailServiceSetting))
            {
                var mailMessage = await BuildMailMessageAsync(_smtpMailServiceSetting, sender, recipients, subject,
                    content,
                    additionalSubjectData, additionalContentData, carbonCopies, blindCarbonCopies, isHtmlContent,
                    cancellationToken);

                await smtpClient.SendMailAsync(mailMessage);
            }
        }

        public virtual async Task SendMailAsync(IMailAddress sender, IMailAddress[] recipients,
            IMailAddress[] carbonCopies,
            IMailAddress[] blindCarbonCopies, string templateName, ExpandoObject additionalSubjectData = null,
            ExpandoObject additionalContentData = null, CancellationToken cancellationToken = default)
        {
            // Find mail template.
            var mail = await GetMailContentAsync(templateName, cancellationToken);

            if (mail == null)
                throw new Exception($"Mail content whose name is {templateName} is not found.");

            using (var smtpClient = GetSmtpClient(_smtpMailServiceSetting))
            {
                var mailMessage = await BuildMailMessageAsync(_smtpMailServiceSetting, sender, recipients, mail.Subject,
                    mail.Content,
                    additionalSubjectData, additionalContentData, carbonCopies, blindCarbonCopies, mail.IsHtml,
                    cancellationToken);

                await smtpClient.SendMailAsync(mailMessage);
            }
        }

        public virtual async Task SendMailAsync(string sender, IMailAddress[] recipients, IMailAddress[] carbonCopies,
            IMailAddress[] blindCarbonCopies, string templateName, ExpandoObject additionalSubjectData = null,
            ExpandoObject additionalContentData = null, CancellationToken cancellationToken = default)
        {
            // Find mail sender.
            var mailSender = await GetSenderAsync(sender, cancellationToken);

            if (mailSender == null)
                throw new Exception($"Mail sender whose name is {sender} is not found.");

            // Find mail template.
            var mail = await GetMailContentAsync(templateName, cancellationToken);

            if (mail == null)
                throw new Exception($"Mail content whose name is {templateName} is not found.");

            using (var smtpClient = GetSmtpClient(_smtpMailServiceSetting))
            {
                var mailMessage = await BuildMailMessageAsync(_smtpMailServiceSetting, mailSender, recipients,
                    mail.Subject, mail.Content,
                    additionalSubjectData, additionalContentData, carbonCopies, blindCarbonCopies, mail.IsHtml,
                    cancellationToken);

                await smtpClient.SendMailAsync(mailMessage);
            }
        }

        public virtual Task<IMailAddress> GetSenderAsync(string key, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IMailContent> GetMailContentAsync(string templateName,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Internal methods

        protected virtual SmtpClient GetSmtpClient(ISmtpMailServiceSetting mailSetting)
        {
            var smtpClient = new SmtpClient(mailSetting.HostName, mailSetting.Port);
            smtpClient.EnableSsl = mailSetting.Ssl;
            smtpClient.Timeout = smtpClient.Timeout;
            smtpClient.Credentials = new NetworkCredential(mailSetting.Username, mailSetting.Password);

            return smtpClient;
        }

        /// <summary>
        ///     Build email using specific settings.
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<MailMessage> BuildMailMessageAsync(
            IMailServiceSetting mailServiceSetting,
            IMailAddress sender, IMailAddress[] recipients,
            string subject, string content,
            ExpandoObject additionalSubjectData = null,
            ExpandoObject additionalContentData = null,
            IMailAddress[] carbonCopies = null,
            IMailAddress[] blindCarbonCopies = null,
            bool isHtml = default,
            CancellationToken cancellationToken = default)
        {
            if (sender == null)
                throw new Exception("Sender is not found");

            if (recipients == null || recipients.Length < 1)
                throw new Exception("No recipient has been defined.");

            var smtpMail = new MailMessage();
            smtpMail.Sender = new MailAddress(sender.Address, sender.DisplayName);
            smtpMail.From = new MailAddress(sender.Address, sender.DisplayName);

            // Recipients
            foreach (var recipient in recipients)
                smtpMail.To.Add(new MailAddress(recipient.Address, recipient.DisplayName));

            var clonedCarbonCopies = (carbonCopies ?? new IMailAddress[0])
                .Concat(mailServiceSetting?.CarbonCopies ?? new IMailAddress[0])
                .ToArray();

            var clonedBlindCarbonCopies = (blindCarbonCopies ?? new IMailAddress[0])
                .Concat(mailServiceSetting?.BlindCarbonCopies ?? new IMailAddress[0])
                .ToArray();

            // Carbon copies
            foreach (var carbonCopy in clonedCarbonCopies)
                smtpMail.CC.Add(new MailAddress(carbonCopy.Address, carbonCopy.DisplayName));

            // Blind carbon copies.
            foreach (var blindCarbonCopy in clonedBlindCarbonCopies)
                smtpMail.Bcc.Add(new MailAddress(blindCarbonCopy.Address,
                    blindCarbonCopy.DisplayName));

            smtpMail.Subject = await RenderContentAsync(subject, additionalSubjectData);
            smtpMail.Body = await RenderContentAsync(content, additionalContentData);
            smtpMail.IsBodyHtml = isHtml;

            return smtpMail;
        }

        /// <summary>
        ///     Render content asynchronously.
        /// </summary>
        /// <param name="initialContent"></param>
        /// <param name="additionalInfo"></param>
        /// <returns></returns>
        protected virtual Task<string> RenderContentAsync(string initialContent, ExpandoObject additionalInfo)
        {
            return Task.FromResult(initialContent);
        }

        #endregion
    }
}