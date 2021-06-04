using CmsModules.ManagementUi.Cqrs.Queries.CorsPolicies;
using FluentValidation;

namespace CmsModules.ManagementUi.Cqrs.QueryValidators.CorsPolicies
{
    public class GetCorsPolicyQueryValidator : AbstractValidator<GetCorsPolicyQuery>
    {
        public GetCorsPolicyQueryValidator()
        {
            RuleFor(query => query.Name)
                .NotEmpty();
        }
    }
}
