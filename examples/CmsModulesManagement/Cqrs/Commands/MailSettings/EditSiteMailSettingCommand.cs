using System;
using CmsModulesManagement.Converters;
using CmsModulesManagement.Models.Entities;
using CmsModulesManagement.Models.Interfaces;
using CmsModulesManagement.ViewModels;
using MailModule.Models.Interfaces;
using MediatR;
using Newtonsoft.Json;

namespace CmsModulesManagement.Cqrs.Commands.MailSettings
{
    public class EditSiteMailSettingCommand : IRequest<SiteMailClientSetting>
    {
        #region Properties

        public Guid Id { get; set; }

        public EditableFieldViewModel<string> DisplayName { get; set; }

        public EditableFieldViewModel<int> Timeout { get; set; }

        [JsonConverter(typeof(MailHostConverter))]
        public IMailHost MailHost { get; set; }

        public EditableFieldViewModel<MailAddressViewModel[]> CarbonCopies { get; set; }

        public EditableFieldViewModel<MailAddressViewModel[]> BlindCarbonCopies { get; set; }

        #endregion
    }
}