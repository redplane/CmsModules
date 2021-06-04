using System.Threading;
using System.Threading.Tasks;
using CmsModulesManagement.Cqrs.Commands.CorsPolicies;
using CmsModulesManagement.Services.Interfaces;
using CorsModule.Models.Interfaces;
using MediatR;

namespace CmsModulesManagement.Cqrs.CommandHandlers.CorsPolicies
{
    public class UpdateCorsPolicyCommandHandler : IRequestHandler<UpdateCorsPolicyCommand, ICorsPolicy>
    {
        #region Properties

        private readonly ISiteCorsPolicyService _siteCorsPolicyService;

        #endregion

        #region Constructor

        public UpdateCorsPolicyCommandHandler(ISiteCorsPolicyService siteCorsPolicyService)
        {
            _siteCorsPolicyService = siteCorsPolicyService;
        }

        #endregion

        #region Methods

        public virtual async Task<ICorsPolicy> Handle(UpdateCorsPolicyCommand command, CancellationToken cancellationToken)
        {
            var updatedCorsPolicy = await _siteCorsPolicyService
                .UpdateSiteCorsPolicyAsync(command.Id, command.AllowedHeaders, 
                command.AllowedOrigins, command.AllowedMethods, command.AllowedExposedHeaders, 
                command.AllowCredential, command.Availability, cancellationToken);

            return updatedCorsPolicy;
        }

        #endregion
    }
}
