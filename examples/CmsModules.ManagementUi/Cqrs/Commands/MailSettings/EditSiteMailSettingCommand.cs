using System;
using CmsModules.ManagementUi.Converters;
using CmsModules.ManagementUi.Models.Entities;
using CmsModules.ManagementUi.ViewModels;
using MailModule.Models.Interfaces;
using MediatR;
using Newtonsoft.Json;

namespace CmsModules.ManagementUi.Cqrs.Commands.MailSettings
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