using System.Threading;
using System.Threading.Tasks;

namespace MailModule.Services.Interfaces
{
    public interface IMailClientsManager
    {
        #region Methods

        /// <summary>
        ///     Get all registered mail services.
        /// </summary>
        /// <returns></returns>
        Task<IMailClient[]> GetMailClientsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        ///     Get a a registered mail service by name.
        /// </summary>
        /// <returns></returns>
        Task<IMailClient> GetMailClientAsync(string uniqueName, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Get mail service is selected.
        /// </summary>
        /// <returns></returns>
        Task<IMailClient> GetActiveMailClientAsync(CancellationToken cancellationToken = default);

        /// <summary>
        ///     Set mail service as active.
        /// </summary>
        Task MarkMailClientAsActiveAsync(string uniqueName, CancellationToken cancellationToken = default);

        #endregion
    }
}