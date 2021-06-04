using CmsModulesManagement.Cqrs.Queries.CorsPolicies;
using FluentValidation;

namespace CmsModulesManagement.Cqrs.QueryValidators.CorsPolicies
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
