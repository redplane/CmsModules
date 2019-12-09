using MailServices.Models.Interfaces;

namespace MailWeb.Models.Interfaces
{
    public interface IMailGunServiceSetting : IMailServiceSetting
    {
        #region Properties

        string Domain { get; }
        
        string ApiKey { get; }
        
        #endregion
    }
}