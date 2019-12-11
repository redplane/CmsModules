using System;
using MailWeb.Models.ValueObjects;
using MailWeb.ViewModels;
using MailWeb.ViewModels.MailSettings;
using MediatR;

namespace MailWeb.Cqrs.Commands.SmtpSettings
{
    public class EditSmtpSettingCommand : IRequest<MailSettingViewModel>
    {
        #region Properties

        public Guid Id { get; set; }

        public EditableFieldViewModel<string> DisplayName { get; set; }

        public EditableFieldViewModel<int> Timeout { get; set; }

        public EditableFieldViewModel<string> HostName { get; set; }

        public EditableFieldViewModel<int> Port { get; set; }

        public EditableFieldViewModel<bool> Ssl { get; set; }

        public EditableFieldViewModel<string> Username { get; set; }

        public EditableFieldViewModel<string> Password { get; set; }

        #endregion
    }
}