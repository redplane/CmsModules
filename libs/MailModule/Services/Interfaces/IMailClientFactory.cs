using System.Threading;
using System.Threading.Tasks;

namespace MailModule.Services.Interfaces
{
    public interface IMailClientFactory
    {
        #region Methods

        /// <summary>
        ///     Get all registered mail services.
        /// </summary>
        /// <returns></returns>
        Task<IMailClient[]> GetMailServicesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        ///     Get a a registered mail service by name.
        /// </summary>
        /// <returns></returns>
        Task<IMailClient> GetMailServiceAsync(string uniqueName, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Get mail service is selected.
        /// </summary>
        /// <returns></returns>
        Task<IMailClient> GetActiveMailClientAsync(CancellationToken cancellationToken = default);

        /// <summary>
        ///     Set mail service as active.
        /// </summary>
        Task SetActiveMailClientAsync(string uniqueName, CancellationToken cancellationToken = default);

        #endregion
    }
}