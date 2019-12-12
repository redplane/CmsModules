using System;
using FluentValidation;
using FluentValidation.Validators;
using MailWeb.Cqrs.Commands.MailSettings;
using MailWeb.Models;

namespace MailWeb.Cqrs.CommandValidators.MailSettings
{
    public class EditMailSettingCommandValidator : AbstractValidator<EditMailSettingCommand>
    {
        public EditMailSettingCommandValidator(
            EditSmtpMailHostCommandValidator editSmtpMailHostCommandValidator,
            EditMailGunHostCommandValidator editMailGunHostCommandValidator)
        {
            RuleFor(command => command.DisplayName.Value)
                .NotEmpty()
                .When(command => command.DisplayName != null && command.DisplayName.HasModified);

            RuleFor(command => command.Timeout.Value)
                .GreaterThan(0)
                .When(command => command.Timeout != null && command.Timeout.HasModified);

            RuleFor(command => (EditSmtpHostModel) command.MailHost)
                .SetValidator(editSmtpMailHostCommandValidator)
                .When(command => command.MailHost is EditSmtpHostModel);

            RuleFor(command => (EditMailGunHostModel) command.MailHost)
                .SetValidator(editMailGunHostCommandValidator)
                .When(command => command.MailHost is EditMailGunHostModel);

        }
    }
}