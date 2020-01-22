using System;
using CmsModulesManagement.Converters;
using CmsModulesManagement.Models.Entities;
using CmsModulesManagement.Models.Interfaces;
using CmsModulesManagement.ViewModels;
using MediatR;
using Newtonsoft.Json;

namespace CmsModulesManagement.Cqrs.Commands.MailSettings
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