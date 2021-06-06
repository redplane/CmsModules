using System;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using GuardNet;
using MailModule.Models.Interfaces;
using MailModule.Services.Interfaces;

namespace MailModule.Services.Implementations
{
    public abstract class DefaultSmtpMailClient : IMailClient
    {
        #region Constructor

        public DefaultSmtpMailClient(IMailClientSetting mailClientSetting)
        {
            _mailClientSetting = mailClientSetting;
        }

        #endregion

        #region Properties

        public abstract string UniqueName { get; }

        public abstract string DisplayName { get; }

        protected readonly IMailClientSetting _mailClientSetting;

        #endregion

        #region Methods

        public virtual async Task SendMailAsync(IMailAddress sender, IMailAddress[] recipients,
            IMailAddress[] carbonCopies,
            IMailAddress[] blindCarbonCopies, string subject, string content, bool isHtmlContent = false,
            object additionalSubjectData = null, object additionalContentData = null,
            Attachment[] attachments = default,
            CancellationToken cancellationToken = default)
        {
            using (var smtpClient = GetSmtpClient(_mailClientSetting))
            {
                var mailMessage = await BuildMailMessageAsync(_mailClientSetting, sender, recipients, subject,
                    content,
                    additionalSubjectData, additionalContentData, carbonCopies, blindCarbonCopies,
                    attachments,
                    isHtmlContent,
                    cancellationToken);

                await smtpClient.SendMailAsync(mailMessage);
            }
        }

        public virtual async Task SendMailAsync(IMailAddress sender, IMailAddress[] recipients,
            IMailAddress[] carbonCopies,
            IMailAddress[] blindCarbonCopies, string templateName, object additionalSubjectData = null,
            object additionalContentData = null,
            Attachment[] attachments = default,
            CancellationToken cancellationToken = default)
        {
            // Find mail template.
            var mail = await GetMailContentAsync(templateName, cancellationToken);

            if (mail == null)
                throw new Exception($"Mail content whose name is {templateName} is not found.");

            using (var smtpClient = GetSmtpClient(_mailClientSetting))
            {
                var mailMessage = await BuildMailMessageAsync(_mailClientSetting, sender, recipients, mail.Subject,
                    mail.Content,
                    additionalSubjectData, additionalContentData, carbonCopies, blindCarbonCopies,
                    attachments,
                    mail.IsHtml,
                    cancellationToken);

                await smtpClient.SendMailAsync(mailMessage);
            }
        }

        public virtual async Task SendMailAsync(string sender, IMailAddress[] recipients, IMailAddress[] carbonCopies,
            IMailAddress[] blindCarbonCopies, string templateName, object additionalSubjectData = null,
            object additionalContentData = null,
            Attachment[] attachments = default,
            CancellationToken cancellationToken = default)
        {
            Guard.NotNullOrWhitespace(sender, nameof(sender));
            Guard.NotNullOrWhitespace(templateName, nameof(templateName));
            Guard.NotNull(recipients, nameof(recipients));
            
            // Find mail sender.
            var mailSender = await GetSenderAsync(sender, cancellationToken);

            if (mailSender == null)
                throw new Exception($"Mail sender whose name is {sender} is not found.");

            // Find mail template.
            var mail = await GetMailContentAsync(templateName, cancellationToken);

            if (mail == null)
                throw new Exception($"Mail content whose name is {templateName} is not found.");

            using (var smtpClient = GetSmtpClient(_mailClientSetting))
            {
                var mailMessage = await BuildMailMessageAsync(_mailClientSetting, mailSender, recipients,
                    mail.Subject, mail.Content,
                    additionalSubjectData, additionalContentData, carbonCopies, blindCarbonCopies,
                    attachments,
                    mail.IsHtml,
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

        protected abstract SmtpClient GetSmtpClient(IMailClientSetting mailClientSetting);

        /// <summary>
        ///     Build email using specific settings.
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<MailMessage> BuildMailMessageAsync(
            IMailClientSetting mailClientSetting,
            IMailAddress sender, IMailAddress[] recipients,
            string subject, string content,
            object additionalSubjectData = null,
            object additionalContentData = null,
            IMailAddress[] carbonCopies = null,
            IMailAddress[] blindCarbonCopies = null,
            Attachment[] attachments = default,
            bool isHtml = default,
            CancellationToken cancellationToken = default)
        {
            Guard.NotNull(sender, nameof(sender));
            Guard.NotNull(recipients, nameof(recipients));
          
            if ( recipients.Length < 1)
                throw new Exception("No recipient has been defined.");

            var smtpMail = new MailMessage();
            smtpMail.Sender = new MailAddress(sender.Address, sender.DisplayName);
            smtpMail.From = new MailAddress(sender.Address, sender.DisplayName);

            // Recipients
            foreach (var recipient in recipients)
                smtpMail.To.Add(new MailAddress(recipient.Address, recipient.DisplayName));

            var clonedCarbonCopies = (carbonCopies ?? new IMailAddress[0])
                .Concat(mailClientSetting?.CarbonCopies ?? new IMailAddress[0])
                .ToArray();

            var clonedBlindCarbonCopies = (blindCarbonCopies ?? new IMailAddress[0])
                .Concat(mailClientSetting?.BlindCarbonCopies ?? new IMailAddress[0])
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

            if (attachments != null && attachments.Length > 0)
                foreach (var attachment in attachments)
                    smtpMail.Attachments.Add(attachment);

            return smtpMail;
        }

        /// <summary>
        ///     Render content asynchronously.
        /// </summary>
        /// <param name="initialContent"></param>
        /// <param name="additionalInfo"></param>
        /// <returns></returns>
        protected virtual Task<string> RenderContentAsync(string initialContent, object additionalInfo)
        {
            return Task.FromResult(initialContent);
        }

        #endregion
    }
}
