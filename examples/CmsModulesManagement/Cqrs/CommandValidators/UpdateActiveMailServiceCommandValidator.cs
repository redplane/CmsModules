using CmsModulesManagement.Cqrs.Commands.ClientSettings;
using FluentValidation;

namespace CmsModulesManagement.Cqrs.CommandValidators
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