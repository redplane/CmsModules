using System;
using System.Threading;
using System.Threading.Tasks;
using CmsModulesManagement.Cqrs.Commands.CorsPolicies;
using CmsModulesManagement.Models.Entities;
using CmsModulesManagement.Services.Interfaces;
using CorsModule.Models.Interfaces;
using MediatR;

namespace CmsModulesManagement.Cqrs.CommandHandlers.CorsPolicies
{
    public class AddCorsPolicyCommandHandler : IRequestHandler<AddCorsPolicyCommand, ICorsPolicy>
    {
        #region Properties

        private readonly ISiteCorsPolicyService _siteCorsPolicyService;

        #endregion

        #region Constructor

        public AddCorsPolicyCommandHandler(ISiteCorsPolicyService siteCorsPolicyService)
        {
            _siteCorsPolicyService = siteCorsPolicyService;
        }

        #endregion

        #region Methods

        public virtual async Task<ICorsPolicy> Handle(AddCorsPolicyCommand request, CancellationToken cancellationToken)
        {
            var corsPolicy = new SiteCorsPolicy(Guid.NewGuid());
            corsPolicy.Name = request.Name;
            corsPolicy.AllowCredential = request.AllowCredential;
            corsPolicy.AllowedExposedHeaders = request.AllowedExposedHeaders;
            corsPolicy.AllowedHeaders = request.AllowedHeaders;
            corsPolicy.AllowedMethods = request.AllowedMethods;
            corsPolicy.AllowedOrigins = request.AllowedOrigins;

            var addedCorsPolicy = await _siteCorsPolicyService
                .AddCorsPolicyAsync(corsPolicy, cancellationToken);

            return addedCorsPolicy;
        }

        #endregion
    }
}
