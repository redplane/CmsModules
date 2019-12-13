using MailManager.Models.Interfaces;

namespace MailWeb.Models.Interfaces
{
    public interface IMailGunClientSetting : IMailClientSetting
    {
        #region Properties

        string Domain { get; }

        string ApiKey { get; }

        #endregion
    }
}