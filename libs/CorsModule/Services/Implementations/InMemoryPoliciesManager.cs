using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CorsModule.Models.Implementations;
using CorsModule.Models.Interfaces;
using CorsModule.Services.Interfaces;

namespace CorsModule.Services.Implementations
{
    public class InMemoryPoliciesManager : ICorsPoliciesManager
    {
        #region Properties

        private readonly LinkedList<ICorsPolicy> _corsPolicies;

        #endregion

        #region Constructor

        public InMemoryPoliciesManager(IEnumerable<ICorsPolicy> corsPolicies)
        {
            _corsPolicies = new LinkedList<ICorsPolicy>(corsPolicies);
            if (_corsPolicies == null)
                _corsPolicies = new LinkedList<ICorsPolicy>();
        }

        #endregion

        #region Methods

        public virtual Task<ICorsPolicy> AddCorsPolicyAsync(string uniqueName, string displayName, string[] allowedHeaders, string[] allowedMethods,
            string[] allowedOrigins, string[] allowedExposedHeaders, bool allowCredentials,
            CancellationToken cancellationToken = default)
        {
            ICorsPolicy corsPolicy = new CorsPolicy(uniqueName);
            corsPolicy.DisplayName = displayName;
            corsPolicy.AllowedHeaders = allowedHeaders;
            corsPolicy.AllowedMethods = allowedMethods;
            corsPolicy.AllowedOrigins = allowedOrigins;
            corsPolicy.AllowedExposedHeaders = allowedExposedHeaders;
            corsPolicy.AllowCredentials = allowCredentials;

            _corsPolicies.AddLast(corsPolicy);
            return Task.FromResult(corsPolicy);
        }

        public virtual Task<long> DeleteCorsPolicyAsync(string uniqueName, CancellationToken cancellationToken = default)
        {
            // Find the index of element whose unique name is similar to searched one.
            var matchedItem = _corsPolicies.FirstOrDefault(policy => policy.UniqueName == uniqueName);
            if (matchedItem == null)
                return Task.FromResult((long) 0);

            _corsPolicies.Remove(matchedItem);
            return Task.FromResult((long) 1);
        }

        public Task<ICorsPolicy[]> GetAddedCorsPoliciesAsync(int skip, int take, CancellationToken cancellationToken = default)
        {
            var corsPolicies = _corsPolicies
                .Skip(skip)
                .Take(take)
                .ToArray();

            return Task.FromResult(corsPolicies);
        }

        #endregion
    }
}