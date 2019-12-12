using MailManager.Models.Interfaces;

namespace MailManager.Models.Implementations
{
    public class SmtpMailServiceSetting : ISmtpMailServiceSetting
    {
        public SmtpMailServiceSetting(string uniqueName, string displayName)
        {
            UniqueName = uniqueName;
            DisplayName = displayName;
            Type = "SMTP";
        }

        public SmtpMailServiceSetting(string uniqueName, string displayName, string type)
        {
            UniqueName = uniqueName;
            DisplayName = displayName;
            Type = type;
        }

        #region Properties

        public string UniqueName { get; }

        public string DisplayName { get; }

        public string Type { get; }

        public int Timeout { get; set; }

        public IMailAddress[] CarbonCopies { get; set; }

        public IMailAddress[] BlindCarbonCopies { get; set; }

        public string HostName { get; set; }

        public int Port { get; set; }

        public bool Ssl { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        #endregion
    }
}