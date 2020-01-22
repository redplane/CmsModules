using MailModule.Models.Interfaces;
using MailWeb.Converters;
using MailWeb.Models.Entities;
using MailWeb.ViewModels;
using MediatR;
using Newtonsoft.Json;

namespace MailWeb.Cqrs.Commands.MailSettings
{
    public class AddSiteMailClientCommand : IRequest<SiteMailClientSetting>
    {
        #region Properties

        public string UniqueName { get; set; }

        public string DisplayName { get; set; }

        public int Timeout { get; set; }

        [JsonConverter(typeof(MailHostConverter))]
        public IMailHost MailHost { get; set; }

        public  MailAddressViewModel[] CarbonCopies { get; set; }

        public MailAddressViewModel[] BlindCarbonCopies { get; set; }

        #endregion
    }
}