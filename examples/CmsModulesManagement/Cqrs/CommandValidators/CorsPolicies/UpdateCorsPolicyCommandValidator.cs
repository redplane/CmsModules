using CmsModulesManagement.Cqrs.Commands.CorsPolicies;
using FluentValidation;

namespace CmsModulesManagement.Cqrs.CommandValidators.CorsPolicies
{
    public class UpdateCorsPolicyCommandValidator : AbstractValidator<UpdateCorsPolicyCommand>
    {
        public UpdateCorsPolicyCommandValidator()
        {

            RuleFor(command => command.AllowedExposedHeaders.Value)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .ForEach(allowedExposedHeader => allowedExposedHeader.NotEmpty())
                .When(command => command.AllowedExposedHeaders != null && command.AllowedExposedHeaders.Value.Length > 0);

            RuleFor(command => command.AllowedHeaders.Value)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .ForEach(allowedHeader => allowedHeader.NotEmpty())
                .When(command => command.AllowedHeaders != null && command.AllowedHeaders.Value.Length > 0);

            RuleFor(command => command.AllowedMethods.Value)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .ForEach(allowedMethod => allowedMethod.NotEmpty())
                .When(command => command.AllowedMethods != null && command.AllowedMethods.Value.Length > 0);

            RuleFor(command => command.AllowedOrigins.Value)
                .ForEach(allowedOrigin => allowedOrigin.NotEmpty())
                .When(command => command.AllowedOrigins != null && command.AllowedOrigins.Value.Length > 0);
        }
    }
}
