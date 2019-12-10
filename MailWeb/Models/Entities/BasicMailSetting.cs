using System;
using MailWeb.Models.Interfaces;
using MailWeb.Models.ValueObjects;

namespace MailWeb.Models.Entities
{
    public class BasicMailSetting : BaseEntity, IBasicMailSetting
    {
        #region Properties

        public string UniqueName { get; }

        public string DisplayName { get; set; }

        public int Timeout { get; set; }

        public string HostName { get; set; }

        public int Port { get; set; }

        public bool Ssl { get; set; }

        public SmtpCredentialValueObject Credential { get; set; }

        #endregion

        #region Constructor

        public BasicMailSetting(Guid id, string uniqueName) : base(id)
        {
            UniqueName = uniqueName;
        }

        public BasicMailSetting(string uniqueName) : base(Guid.NewGuid())
        {
            UniqueName = uniqueName;
        }

        #endregion
    }
}