using System.Threading;
using System.Threading.Tasks;
using CorsModule.Models.Interfaces;

namespace CorsModule.Services.Interfaces
{
    public interface ICorsPoliciesManager
    {
        #region Properties

        /// <summary>
        /// Add cors policy asynchronously.
        /// </summary>
        /// <returns></returns>
        Task<ICorsPolicy> AddCorsPolicyAsync(string uniqueName, string displayName, string[] allowedHeaders, string[] allowedMethods,
            string[] allowedOrigins, string[] allowedExposedHeaders, 
            bool allowCredentials, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete cors policy asynchronously.
        /// </summary>
        /// <param name="uniqueName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<long> DeleteCorsPolicyAsync(string uniqueName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get added cors policies asynchronously.
        /// </summary>
        /// <returns></returns>
        Task<ICorsPolicy[]> GetAddedCorsPoliciesAsync(int skip, int take, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get active cors policies asynchronously.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ICorsPolicy[]> GetActiveCorsPoliciesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Set active cors policy asynchronously.
        /// </summary>
        /// <param name="uniqueName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task SetActiveCorsPolicyAsync(string uniqueName, CancellationToken cancellationToken = default);

        #endregion
    }
}