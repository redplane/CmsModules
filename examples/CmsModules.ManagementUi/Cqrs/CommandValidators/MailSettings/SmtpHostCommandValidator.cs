using CmsModules.Shares.Models.MailHosts;
using FluentValidation;

namespace CmsModules.ManagementUi.Cqrs.CommandValidators.MailSettings
{
    public class EditSmtpMailHostCommandValidator : AbstractValidator<SmtpHost>
    {
        public EditSmtpMailHostCommandValidator()
        {
            RuleFor(command => command.Username)
                .NotEmpty();

            RuleFor(command => command.Password)
                .NotEmpty();

            RuleFor(command => command.HostName)
                .NotEmpty();

            RuleFor(command => command.Port)
                .GreaterThan(0);
        }
    }
}