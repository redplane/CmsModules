using MailWeb.Constants;

namespace MailWeb.Models.MailHosts
{
    public class MailGunHost : MailHost
    {
        #region Constructor

        public MailGunHost()
        {
            
        }
        
        #endregion
        
        #region Properties

        public string ApiKey { get; set; }
        
        public string Domain { get; set; }

        public override string Type => MailHostKindConstants.MailGun;

        #endregion
    }
}