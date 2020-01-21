using System;
using System.Threading;
using System.Threading.Tasks;
using CorsModule.Models.Interfaces;
using CorsModule.Services.Interfaces;
using MailWeb.Cqrs.Commands.CorsPolicies;
using MailWeb.Models.Entities;
using MediatR;

namespace MailWeb.Cqrs.CommandHandlers.CorsPolicies
{
    public class AddCorsPolicyCommandHandler : IRequestHandler<AddCorsPolicyCommand, ICorsPolicy>
    {
        #region Properties

        private readonly ICorsPoliciesManager _corsPoliciesManager;

        #endregion

        #region Constructor

        public AddCorsPolicyCommandHandler(ICorsPoliciesManager corsPoliciesManager)
        {
            _corsPoliciesManager = corsPoliciesManager;
        }

        #endregion

        #region Methods

        public virtual async Task<ICorsPolicy> Handle(AddCorsPolicyCommand request, CancellationToken cancellationToken)
        {
            var corsPolicy = new CorsPolicy(Guid.NewGuid());
            corsPolicy.Name = request.Name;
            corsPolicy.AllowCredential = request.AllowCredential;
            corsPolicy.AllowedExposedHeaders = request.AllowedExposedHeaders;
            corsPolicy.AllowedHeaders = request.AllowedHeaders;
            corsPolicy.AllowedMethods = request.AllowedMethods;
            corsPolicy.AllowedOrigins = request.AllowedOrigins;

            var addedCorsPolicy = await _corsPoliciesManager
                .AddCorsPolicyAsync(corsPolicy, cancellationToken);

            return addedCorsPolicy;
        }

        #endregion
    }
}
