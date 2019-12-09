using System;
using MailWeb.Models.Interfaces;
using MailWeb.Models.ValueObjects;

namespace MailWeb.Models.Entities
{
    public class BasicMailSetting : IBasicMailSetting
    {
        #region Properties

        public Guid Id { get; }

        public string UniqueName { get; }

        public string DisplayName { get; set; }

        public int Timeout { get; set; }

        public string HostName { get; set; }

        public int Port { get; set; }

        public bool Ssl { get; set; }

        public MailAccountValueObject Credential { get; set; }

        #endregion

        #region Constructor

        public BasicMailSetting(Guid id, string uniqueName)
        {
            Id = id;
            UniqueName = uniqueName;
        }

        public BasicMailSetting(string uniqueName)
        {
            Id = Guid.NewGuid();
            UniqueName = uniqueName;
        }

        #endregion
    }
}