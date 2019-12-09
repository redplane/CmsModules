namespace MailServices.Models.Interfaces
{
    public interface ISmtpMailServiceSetting : IMailServiceSetting
    {
        #region Properties

        string HostName { get; set; }

        int Port { get; set; }

        bool Ssl { get; set; }

        string Username { get; set; }

        string Password { get; set; }

        #endregion
    }
}