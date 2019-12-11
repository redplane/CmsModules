using FluentValidation;
using MailWeb.Cqrs.Commands;
using MailWeb.Cqrs.Commands.ClientSettings;

namespace MailWeb.Cqrs.CommandValidators
{
    public class UpdateActiveMailServiceCommandValidator : AbstractValidator<UpdateActiveMailServiceCommand>
    {
        public UpdateActiveMailServiceCommandValidator()
        {
            RuleFor(command => command.MailServiceUniqueName)
                .NotEmpty();
        }
    }
}