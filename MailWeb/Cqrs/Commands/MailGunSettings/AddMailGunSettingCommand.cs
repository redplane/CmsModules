using MailWeb.Models.Interfaces;
using MediatR;

namespace MailWeb.Cqrs.Commands.MailGunSettings
{
    public class AddMailGunSettingCommand : IRequest<IBasicMailSetting>
    {
        #region Properties

        public string UniqueName { get; set; }

        public string DisplayName { get; set; }

        public int Timeout { get; set; }

        public string Domain { get; set; }

        public string ApiKey { get; set; }

        #endregion
    }
}