using System;
using MailWeb.Models.Entities;
using MailWeb.Models.ValueObjects;
using MailWeb.ViewModels;
using MailWeb.ViewModels.BasicMailSettings;
using MediatR;

namespace MailWeb.Cqrs.Commands
{
    public class EditBasicMailSettingCommand : IRequest<BasicMailSettingViewModel>
    {
        #region Properties

        public Guid Id { get; }

        public EditableFieldViewModel<string> DisplayName { get; set; }

        public EditableFieldViewModel<int> Timeout { get; set; }

        public EditableFieldViewModel<string> HostName { get; set; }

        public EditableFieldViewModel<int> Port { get; set; }

        public EditableFieldViewModel<bool> Ssl { get; set; }

        public EditableFieldViewModel<SmtpCredentialValueObject> Credential { get; set; }

        #endregion

        #region Constructor

        public EditBasicMailSettingCommand(Guid id)
        {
            Id = id;
        }

        #endregion
    }
}