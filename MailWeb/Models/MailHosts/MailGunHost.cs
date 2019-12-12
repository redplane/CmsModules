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
        
        #endregion
    }
}