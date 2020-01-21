using System.Collections.Generic;
using System.Threading.Tasks;
using CorsModule.Models.Interfaces;
using CorsModule.Services.Interfaces;
using MailWeb.Models;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace MailWeb.Services.Implementations
{
    public class SiteCorsPolicyProvider : ICorsPolicyProvider
    {
        #region Properties

        private readonly ICorsPoliciesManager _corsPoliciesManager;

        #endregion

        #region Constructor

        public SiteCorsPolicyProvider(ICorsPoliciesManager corsPoliciesManager)
        {
            _corsPoliciesManager = corsPoliciesManager;
        }

        #endregion

        #region Methods

        public virtual async Task<CorsPolicy> GetPolicyAsync(HttpContext context, string policyName)
        {
            ICorsPolicy loadedCorsPolicy = null;

            // Policy name is defined, find the entity.
            if (!string.IsNullOrWhiteSpace(policyName))
                loadedCorsPolicy = await _corsPoliciesManager.GetCorsPolicyAsync(policyName);
            else
                loadedCorsPolicy = await _corsPoliciesManager.GetActiveCorsPolicyAsync();

            if (loadedCorsPolicy == null)
                return null;
            
            var appliedCorsPolicyBuilder = new CorsPolicyBuilder();

            var allowedHeaders = loadedCorsPolicy.AllowedHeaders;
            if (allowedHeaders != null && allowedHeaders.Length > 0)
                appliedCorsPolicyBuilder = appliedCorsPolicyBuilder.WithHeaders(allowedHeaders);

            var allowedOrigins = loadedCorsPolicy.AllowedOrigins;
            if (allowedOrigins != null && allowedOrigins.Length > 0)
                appliedCorsPolicyBuilder = appliedCorsPolicyBuilder.WithOrigins(allowedOrigins);

            var allowedMethods = loadedCorsPolicy.AllowedMethods;
            if (allowedMethods != null && allowedMethods.Length > 0)
                appliedCorsPolicyBuilder = appliedCorsPolicyBuilder.WithMethods(allowedMethods);

            var allowedExposedHeaders = loadedCorsPolicy.AllowedExposedHeaders;
            if (allowedExposedHeaders != null && allowedExposedHeaders.Length > 0)
                appliedCorsPolicyBuilder = appliedCorsPolicyBuilder.WithExposedHeaders(allowedExposedHeaders);

            return appliedCorsPolicyBuilder
                .Build();
        }

        #endregion
    }
}
