using System;
using MailWeb.Models.Entities;
using MailWeb.Models.Interfaces;

namespace MailWeb.ViewModels.MailSettings
{
    public class MailSettingViewModel : IBasicMailSetting
    {
        #region Properties

        public Guid Id { get; }

        public string UniqueName { get; }

        public string DisplayName { get; set; }

        public int Timeout { get; set; }
        
        public IMailHost MailHost { get; set; }
        
        #endregion

        #region Constructor

        public MailSettingViewModel(Guid id, string uniqueName)
        {
            Id = id;
            UniqueName = uniqueName;
        }

        public MailSettingViewModel(string uniqueName)
        {
            Id = Guid.NewGuid();
            UniqueName = uniqueName;
        }

        public MailSettingViewModel(MailSetting model)
        {
            Id = model.Id;
            UniqueName = model.UniqueName;
            DisplayName = model.DisplayName;
            Timeout = model.Timeout;
            MailHost = model.MailHost;
        }

        #endregion
    }
}