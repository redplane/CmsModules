using MailClients.Models.Interfaces;
using MailWeb.Converters;
using MailWeb.Models.Entities;
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