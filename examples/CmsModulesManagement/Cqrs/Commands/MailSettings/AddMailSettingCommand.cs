using CmsModulesManagement.Converters;
using CmsModulesManagement.Models.Entities;
using MailModule.Models.Interfaces;
using MediatR;
using Newtonsoft.Json;

namespace CmsModulesManagement.Cqrs.Commands.MailSettings
{
    public class AddMailSettingCommand : IRequest<MailClientSetting>
    {
        #region Properties

        public string UniqueName { get; set; }

        public string DisplayName { get; set; }

        public int Timeout { get; set; }

        [JsonConverter(typeof(MailHostConverter))]
        public IMailHost MailHost { get; set; }

        #endregion
    }
}