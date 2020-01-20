using System.Net;
using System.Net.Mail;
using CmsModulesShared.Models.MailHosts;
using MailModule.Models.Interfaces;
using MailModule.Services.Implementations;

namespace MailWeb.Services.Implementations
{
    public class OutlookMailClient : DefaultSmtpMailClient
    {
        #region Constructor

        public OutlookMailClient(IMailClientSetting smtpMailSetting) : base(smtpMailSetting)
        {
            _mailHost = (SmtpHost) smtpMailSetting.MailHost;
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