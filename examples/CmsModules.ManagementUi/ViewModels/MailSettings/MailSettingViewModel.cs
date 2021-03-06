﻿using System;
using CmsModules.ManagementUi.Models.Entities;
using MailModule.Models.Interfaces;

namespace CmsModules.ManagementUi.ViewModels.MailSettings
{
    public class MailSettingViewModel : IMailClientSetting
    {
        #region Properties

        public Guid Id { get; }

        public string UniqueName { get; }

        public string DisplayName { get; set; }

        public string Type { get; set; }

        public int Timeout { get; set; }

        public IMailAddress[] CarbonCopies { get; set; }

        public IMailAddress[] BlindCarbonCopies { get; set; }

        public IMailHost MailHost { get; set; }

        public Guid TenantId { get; set; }
        
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

        public MailSettingViewModel(SiteMailClientSetting model)
        {
            Id = model.Id;
            UniqueName = model.UniqueName;
            DisplayName = model.DisplayName;
            Timeout = model.Timeout;
            MailHost = model.MailHost;
            TenantId = model.TenantId;
        }

        #endregion
    }
}