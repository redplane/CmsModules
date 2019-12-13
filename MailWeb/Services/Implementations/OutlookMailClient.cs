using System.Net;
using System.Net.Mail;
using MailManager.Models.Interfaces;
using MailManager.Services.Implementations;
using MailWeb.Models.Interfaces;
using MailWeb.Models.MailHosts;

namespace MailWeb.Services.Implementations
{
    public class OutlookMailClient : BaseSmtpMailClient
    {
        #region Constructor

        public OutlookMailClient(IMailClientSetting smtpMailSetting, IMailHost mailHost) : base(smtpMailSetting)
        {
            _mailHost = (SmtpHost) mailHost;
        }

        #endregion

        #region Properties

        private readonly SmtpHost _mailHost;

        public override string UniqueName => "Outlook";

        public override string DisplayName => "Outlook mail service";
        
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
    }
}