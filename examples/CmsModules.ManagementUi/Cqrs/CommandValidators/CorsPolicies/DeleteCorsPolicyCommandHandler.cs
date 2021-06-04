using System.Threading;
using System.Threading.Tasks;
using CmsModules.ManagementUi.Cqrs.Commands.CorsPolicies;
using CmsModules.ManagementUi.Services.Interfaces;
using MediatR;

namespace CmsModules.ManagementUi.Cqrs.CommandValidators.CorsPolicies
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
