using System;
using MailManager.Models.Interfaces;
using MailWeb.Converters;
using MailWeb.Enums;
using Newtonsoft.Json;

namespace MailWeb.ViewModels
{
    public class MailClientSettingViewModel
    {
        #region Constructor


        #endregion

        #region Properties

        public Guid Id { get; set; }

        public MasterItemAvailabilities Availability { get; set; }

        public double CreatedTime { get; set; }

        public double? LastModifiedTime { get; set; }

        public Guid ClientId { get; set; }

        public string UniqueName { get; set; }

        public string DisplayName { get; set; }

        public int Timeout { get; set; }

        public string CarbonCopies { get; set; }

        public string BlindCarbonCopies { get; set; }

        public string MailHost { get; set; }

        #endregion
    }
}