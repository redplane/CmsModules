using System.Threading;
using System.Threading.Tasks;

namespace DataMagic.Abstractions.Interfaces
{
    public interface ISearchResult<T>
    {
        #region Properties

        T[] Items { get; }

        long TotalRecords { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Get the first item from the search result.
        /// If item is not available, exception will be thrown.
        /// </summary>
        /// <returns></returns>
        T First();

        /// <summary>
        /// Get the first item from the search result asynchronously.
        /// If item is not available, exception will be thrown.
        /// </summary>
        /// <returns></returns>
        Task<T> FirstAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the first item from the search result.
        /// If item is not found, default value will be returned.
        /// </summary>
        /// <returns></returns>
        T FirstOrDefault();

        /// <summary>
        /// Get the first item from the search result asynchronously.
        /// If item is not found, default value will be returned.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> FirstOrDefaultAsync(CancellationToken cancellationToken = default);

        #endregion
    }
}