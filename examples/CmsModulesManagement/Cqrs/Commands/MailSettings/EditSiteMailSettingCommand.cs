using System;
using MailModule.Models.Interfaces;
using MailWeb.Converters;
using MailWeb.Models.Entities;
using MailWeb.Models.Interfaces;
using MailWeb.ViewModels;
using MediatR;
using Newtonsoft.Json;

namespace MailWeb.Cqrs.Commands.MailSettings
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