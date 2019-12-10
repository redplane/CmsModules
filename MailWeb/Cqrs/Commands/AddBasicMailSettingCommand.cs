using MailWeb.Models.ValueObjects;
using MailWeb.ViewModels;
using MailWeb.ViewModels.BasicMailSettings;
using MediatR;

namespace MailWeb.Cqrs.Commands
{
    public class AddBasicMailSettingCommand : IRequest<BasicMailSettingViewModel>
    {
        #region Properties

        public string UniqueName { get; set; }

        public string DisplayName { get; set; }

        public int Timeout { get; set; }

        public string HostName { get; set; }

        public int Port { get; set; }

        public bool Ssl { get; set; }

        public SmtpCredentialValueObject Credential { get; set; }

        #endregion
    }
}