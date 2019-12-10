using FluentValidation;
using MailWeb.Cqrs.Commands;

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