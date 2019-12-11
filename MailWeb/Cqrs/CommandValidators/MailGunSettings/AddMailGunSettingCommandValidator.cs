using FluentValidation;
using MailWeb.Cqrs.Commands.MailGunSettings;

namespace MailWeb.Cqrs.CommandValidators.MailGunSettings
{
    public class AddMailGunSettingCommandValidator : AbstractValidator<AddMailGunSettingCommand>
    {
        public AddMailGunSettingCommandValidator()
        {
            RuleFor(command => command.UniqueName)
                .NotEmpty();

            RuleFor(command => command.DisplayName)
                .NotEmpty();

            RuleFor(command => command.Domain)
                .NotEmpty();

            RuleFor(command => command.ApiKey)
                .NotEmpty();
        }
    }
}