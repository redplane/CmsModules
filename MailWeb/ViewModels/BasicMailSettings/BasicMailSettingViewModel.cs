using System;
using MailWeb.Models.Entities;
using MailWeb.Models.Interfaces;
using MailWeb.Models.ValueObjects;

namespace MailWeb.ViewModels.BasicMailSettings
{
    public class BasicMailSettingViewModel : IBasicMailSetting
    {
        #region Properties

        public Guid Id { get; }

        public string UniqueName { get; }

        public string DisplayName { get; set; }

        public int Timeout { get; set; }
        
        public IMailHost MailHost { get; set; }
        
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
            MailHost = model.MailHost;
        }

        #endregion
    }
}