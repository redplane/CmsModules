using MailServices.Models.Interfaces;
using MailWeb.Models.Interfaces;

namespace MailWeb.Models
{
    public class MailGunServiceSetting : IMailGunServiceSetting
    {
        #region Constructor

        public MailGunServiceSetting(string uniqueName, string displayName, string domain, string apiKey)
        {
            UniqueName = uniqueName;
            DisplayName = displayName;
            Domain = domain;
            ApiKey = apiKey;
        }

        #endregion

        #region Properties

        public string UniqueName { get; }

        public string DisplayName { get; }

        public string Type => "MailGun";

        public int Timeout { get; set; }

        public IMailAddress[] CarbonCopies { get; set; }

        public IMailAddress[] BlindCarbonCopies { get; set; }

        public string Domain { get; }

        public string ApiKey { get; }

        #endregion
    }
}