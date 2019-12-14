using System;
using MailWeb.Converters;
using MailWeb.Models.Entities;
using MailWeb.Models.Interfaces;
using MailWeb.ViewModels;
using MediatR;
using Newtonsoft.Json;

namespace MailWeb.Cqrs.Commands.MailSettings
{
    public class EditMailSettingCommand : IRequest<MailClientSetting>
    {
        #region Properties

        public Guid Id { get; set; }

        public EditableFieldViewModel<string> DisplayName { get; set; }

        public EditableFieldViewModel<int> Timeout { get; set; }

        [JsonConverter(typeof(EditMailHostConverter))]
        public IEditMailHost MailHost { get; set; }

        #endregion
    }
}