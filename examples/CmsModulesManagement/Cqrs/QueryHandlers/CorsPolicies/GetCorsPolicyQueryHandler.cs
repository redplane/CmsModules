using System.Threading;
using System.Threading.Tasks;
using CorsModule.Models.Interfaces;
using MailWeb.Cqrs.Queries.CorsPolicies;
using MailWeb.Services.Interfaces;
using MediatR;

namespace MailWeb.Cqrs.QueryHandlers.CorsPolicies
{
    public class GetCorsPolicyQueryHandler : IRequestHandler<GetCorsPolicyQuery, ICorsPolicy>
    {
        #region Properties

        private readonly ISiteCorsPolicyService _corsPoliciesManager;

        #endregion

        #region Constructor

        public GetCorsPolicyQueryHandler(ISiteCorsPolicyService corsPoliciesManager)
        {
            _corsPoliciesManager = corsPoliciesManager;
        }

        #endregion

        #region Methods

        public virtual async Task<ICorsPolicy> Handle(GetCorsPolicyQuery request, CancellationToken cancellationToken)
        {
            var corsPolicy = await _corsPoliciesManager
                .GetCorsPolicyAsync(request.Name, cancellationToken);

            return corsPolicy;
        }

        #endregion
    }
}
