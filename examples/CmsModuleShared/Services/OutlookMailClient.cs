using System;
using System.Net;
using System.Net.Mail;
using CmsModuleShared.Models.MailHosts;
using MailModule.Models.Interfaces;
using MailModule.Services.Implementations;

namespace CmsModuleShared.Services
{
    public class OutlookMailClient : BaseSmtpMailClient
    {
        #region Constructor

        public OutlookMailClient(IMailClientSetting smtpMailSetting) : base(smtpMailSetting)
        {
            var mailHost = smtpMailSetting.MailHost;
            if (!(mailHost is SmtpHost smtpHost))
                throw new ArgumentException($"{nameof(smtpMailSetting.MailHost)} must be an instance of {nameof(SmtpHost)}");

            _mailHost = smtpHost;
        }

        #endregion

        #region Methods

        protected override SmtpClient GetSmtpClient(IMailClientSetting mailClientSetting)
        {
            var smtpClient = new SmtpClient();
            smtpClient.Host = _mailHost.HostName;
            smtpClient.Port = _mailHost.Port;
            smtpClient.Credentials = new NetworkCredential(_mailHost.Username, _mailHost.Password);
            smtpClient.Timeout = _mailClientSetting.Timeout;
            smtpClient.EnableSsl = _mailHost.Ssl;

            return smtpClient;
        }

        #endregion

        #region Properties

        private readonly SmtpHost _mailHost;

        public override string UniqueName => _mailClientSetting.UniqueName;

        public override string DisplayName => _mailClientSetting.DisplayName;

        #endregion
    }
}