using CmsModulesShared.Constants;

namespace CmsModulesShared.Models.MailHosts
{
    public class SmtpHost : MailHost
    {
        #region Properties

        public string HostName { get; set; }

        public int Port { get; set; }

        public bool Ssl { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public override string Type => MailHostKindConstants.Smtp;

        #endregion

        #region Constructor

        #endregion
    }
}