using System.Threading;
using System.Threading.Tasks;
using CorsModule.Models.Interfaces;

namespace CorsModule.Services.Interfaces
{
    public interface ICorsPoliciesManager
    {
        #region Methods

        /// <summary>
        /// Mark a cors policy as active asynchronously.
        /// </summary>
        /// <param name="corsPolicy"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task MarkCorsPolicyAsActiveAsync(ICorsPolicy corsPolicy, CancellationToken cancellationToken = default);

        /// <summary>
        /// Add cors policy into system asynchronously.
        /// </summary>
        /// <param name="corsPolicy"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ICorsPolicy> AddCorsPolicyAsync(ICorsPolicy corsPolicy, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete cors policy asynchronously.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeleteCorsPolicyAsync(string name, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get cors policy asynchronously.
        /// </summary>
        /// <returns></returns>
        Task<ICorsPolicy[]> GetInUseCorsPoliciesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get cors policy by using unique name asynchronously.
        /// </summary>
        /// <param name="uniqueName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ICorsPolicy> GetCorsPolicyAsync(string uniqueName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get cors policy asynchronously.
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ICorsPolicy[]> GetCorsPoliciesAsync(int skip, int? take, CancellationToken cancellationToken = default);

        #endregion
    }
}