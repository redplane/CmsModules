using System.Threading;
using System.Threading.Tasks;
using CorsModule.Models.Interfaces;
using CorsModule.Services.Interfaces;
using MailWeb.Cqrs.Commands.CorsPolicies;
using MailWeb.Services.Interfaces;
using MediatR;

namespace MailWeb.Cqrs.CommandValidators.CorsPolicies
{
    public class DeleteCorsPolicyCommandHandler : IRequestHandler<DeleteCorsPolicyCommand, bool>
    {
        #region Properties

        private readonly ISiteCorsPolicyService _corsPoliciesManager;

        #endregion

        #region Constructor

        public DeleteCorsPolicyCommandHandler(ISiteCorsPolicyService corsPoliciesManager)
        {
            _corsPoliciesManager = corsPoliciesManager;
        }

        #endregion

        #region Methods

        public virtual async Task<bool> Handle(DeleteCorsPolicyCommand request, CancellationToken cancellationToken)
        {
            await _corsPoliciesManager
                .DeleteCorsPolicyAsync(request.UniqueName, cancellationToken);
            return true;
        }

        #endregion
    }
}
