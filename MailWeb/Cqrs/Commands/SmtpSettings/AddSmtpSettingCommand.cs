using MailWeb.Models.ValueObjects;
using MailWeb.ViewModels.BasicMailSettings;
using MediatR;

namespace MailWeb.Cqrs.Commands.SmtpSettings
{
    public class AddSmtpSettingCommand : IRequest<BasicMailSettingViewModel>
    {
        #region Properties

        public string UniqueName { get; set; }

        public string DisplayName { get; set; }

        public int Timeout { get; set; }

        public string HostName { get; set; }

        public int Port { get; set; }

        public bool Ssl { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        #endregion
    }
}