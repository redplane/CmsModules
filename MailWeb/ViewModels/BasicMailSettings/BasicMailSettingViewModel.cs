using System;
using MailWeb.Models.Entities;
using MailWeb.Models.ValueObjects;

namespace MailWeb.ViewModels.BasicMailSettings
{
    public class BasicMailSettingViewModel
    {
        #region Properties

        public Guid Id { get; private set; }

        public string UniqueName { get; }

        public string DisplayName { get; set; }

        public int Timeout { get; set; }

        public string HostName { get; set; }

        public int Port { get; set; }

        public bool Ssl { get; set; }

        public SmtpCredentialValueObject Credential { get; set; }

        #endregion

        #region Constructor

        public BasicMailSettingViewModel(Guid id, string uniqueName)
        {
            Id = id;
            UniqueName = uniqueName;
        }

        public BasicMailSettingViewModel(string uniqueName)
        {
            Id = Guid.NewGuid();
            UniqueName = uniqueName;
        }

        public BasicMailSettingViewModel(BasicMailSetting model)
        {
            Id = model.Id;
            UniqueName = model.UniqueName;
            DisplayName = model.DisplayName;
            Timeout = model.Timeout;
            HostName = model.HostName;
            Port = model.Port;
            Ssl = model.Ssl;
            Credential = model.Credential;
        }

        #endregion
    }
}