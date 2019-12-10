using FluentValidation;
using MailWeb.Cqrs.Commands;

namespace MailWeb.Cqrs.CommandValidators
{
    public class AddBasicMailSettingCommandValidator : AbstractValidator<AddBasicMailSettingCommand>
    {
        public AddBasicMailSettingCommandValidator()
        {
            RuleFor(command => command.UniqueName)
                .NotEmpty();

            RuleFor(command => command.DisplayName)
                .NotEmpty();

            RuleFor(command => command.Timeout)
                .GreaterThan(-1);

            RuleFor(command => command.Credential)
                .NotNull();
        }
    }
}