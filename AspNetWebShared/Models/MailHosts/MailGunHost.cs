using AspNetWebShared.Constants;

namespace AspNetWebShared.Models.MailHosts
{
    public class MailGunHost : MailHost
    {
        #region Constructor

        #endregion

        #region Properties

        public string ApiKey { get; set; }

        public string Domain { get; set; }

        public override string Type => MailHostKindConstants.MailGun;

        #endregion
    }
}