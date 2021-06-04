using System;
using System.Threading.Tasks;
using CmsModules.ManagementUi.Services.Interfaces;
using CorsModule.Models.Interfaces;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CmsModules.ManagementUi.Services.Implementations
{
    public class SiteCorsPolicyProvider : ICorsPolicyProvider
    {

        #region Constructor

        public SiteCorsPolicyProvider()
        {
        }

        #endregion

        #region Methods

        public virtual async Task<CorsPolicy> GetPolicyAsync(HttpContext context, string policyName)
        {
            ICorsPolicy[] loadedCorsPolicies = null;

            // Find cors policies manager.
            var corsPoliciesManager = context.RequestServices.GetService<ISiteCorsPolicyService>();

            if (corsPoliciesManager == null)
                throw new ArgumentException($"{nameof(ISiteCorsPolicyService)} is not found in service context.");

            // Policy name is defined, find the entity.
            if (!string.IsNullOrWhiteSpace(policyName))
            {
                var loadedCorsPolicy = await corsPoliciesManager.GetCorsPolicyAsync(policyName);
                if (loadedCorsPolicy != null)
                    loadedCorsPolicies = new[] { loadedCorsPolicy };
            }
            else
                loadedCorsPolicies = await corsPoliciesManager.GetInUseCorsPoliciesAsync();

            if (loadedCorsPolicies == null || loadedCorsPolicies.Length < 1)
                return null;

            var appliedCorsPolicyBuilder = new CorsPolicyBuilder();

            foreach (var loadedCorsPolicy in loadedCorsPolicies)
            {
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

                if (loadedCorsPolicy.AllowCredential)
                    appliedCorsPolicyBuilder = appliedCorsPolicyBuilder.AllowCredentials();
            }

            var builtPolicy = appliedCorsPolicyBuilder
                .Build();

            return builtPolicy;
        }

        #endregion
    }
}
