using FluentValidation;
using MailWeb.Cqrs.Commands.SmtpSettings;

namespace MailWeb.Cqrs.CommandValidators
{
    public class AddSmtpSettingCommandValidator : AbstractValidator<AddSmtpSettingCommand>
    {
        public AddSmtpSettingCommandValidator()
        {
            RuleFor(command => command.UniqueName)
                .NotEmpty();

            RuleFor(command => command.DisplayName)
                .NotEmpty();

            RuleFor(command => command.Timeout)
                .GreaterThan(-1);

            RuleFor(command => command.Username)
                .NotEmpty();

            RuleFor(command => command.Password)
                .NotEmpty();
        }
    }
}