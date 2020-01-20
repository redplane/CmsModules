using FluentValidation;
using MailWeb.Models;

namespace MailWeb.Cqrs.CommandValidators.MailSettings
{
    public class EditSmtpMailHostCommandValidator : AbstractValidator<EditSmtpHostModel>
    {
        public EditSmtpMailHostCommandValidator()
        {
            RuleFor(command => command.Username.Value)
                .NotEmpty()
                .When(command => command.Username != null && command.Username.HasModified);

            RuleFor(command => command.Password.Value)
                .NotEmpty()
                .When(command => command.Password != null && command.Password.HasModified);

            RuleFor(command => command.HostName.Value)
                .NotEmpty()
                .When(command => command.HostName != null && command.HostName.HasModified);

            RuleFor(command => command.Port.Value)
                .GreaterThan(0)
                .When(command => command.Port != null && command.Port.HasModified);
        }
    }
}