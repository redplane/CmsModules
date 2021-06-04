using CmsModules.ManagementUi.Cqrs.Commands.CorsPolicies;
using FluentValidation;

namespace CmsModules.ManagementUi.Cqrs.CommandValidators.CorsPolicies
{
    public class AddCorsPolicyCommandValidator : AbstractValidator<AddCorsPolicyCommand>
    {
        public AddCorsPolicyCommandValidator()
        {
            RuleFor(command => command.Name)
                .NotEmpty();

            RuleFor(command => command.AllowedExposedHeaders)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .ForEach(allowedExposedHeader => allowedExposedHeader.NotEmpty())
                .When(command => command.AllowedExposedHeaders != null && command.AllowedExposedHeaders.Length > 0);

            RuleFor(command => command.AllowedHeaders)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .ForEach(allowedHeader => allowedHeader.NotEmpty())
                .When(command => command.AllowedHeaders != null && command.AllowedHeaders.Length > 0);

            RuleFor(command => command.AllowedMethods)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .ForEach(allowedMethod => allowedMethod.NotEmpty())
                .When(command => command.AllowedMethods != null && command.AllowedMethods.Length > 0);

            RuleFor(command => command.AllowedOrigins)
                .ForEach(allowedOrigin => allowedOrigin.NotEmpty())
                .When(command => command.AllowedOrigins != null && command.AllowedOrigins.Length > 0);
        }
    }
}
