using CorsModule.Models.Interfaces;
using FluentValidation;
using MailWeb.Cqrs.Queries.CorsPolicies;

namespace MailWeb.Cqrs.QueryValidators.CorsPolicies
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
