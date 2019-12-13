using System.Security;
using MailWeb.Converters;
using MailWeb.Models.Entities;
using MailWeb.Models.Interfaces;
using MediatR;
using Newtonsoft.Json;

namespace MailWeb.Cqrs.Commands.MailSettings
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