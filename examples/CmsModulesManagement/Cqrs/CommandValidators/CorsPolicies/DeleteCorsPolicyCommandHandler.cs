using System.Threading;
using System.Threading.Tasks;
using CmsModulesManagement.Cqrs.Commands.CorsPolicies;
using CmsModulesManagement.Services.Interfaces;
using MediatR;

namespace CmsModulesManagement.Cqrs.CommandValidators.CorsPolicies
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
