using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataMagic.Abstractions.Interfaces;

namespace DataMagic.Abstractions.Models
{
    public class SearchResult<T> : ISearchResult<T>
    {
        #region Constructor

        public SearchResult(IEnumerable<T> items, int totalRecords)
        {
            Items = items?.ToArray() ?? new T[0];
            TotalRecords = totalRecords;
        }

        public SearchResult(IEnumerable<T> items, long totalRecords)
        {
            Items = items?.ToArray() ?? new T[0];
            TotalRecords = totalRecords;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     <inheritdoc />
        /// </summary>
        public T[] Items { get; }

        /// <summary>
        ///     Total records.
        /// </summary>
        public long TotalRecords { get; }

        #endregion

        #region Methods

        /// <summary>
        ///     <inheritdoc />
        /// </summary>
        /// <returns></returns>
        public virtual T First()
        {
            return Items.First();
        }

        /// <summary>
        ///     <inheritdoc />
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<T> FirstAsync(CancellationToken cancellationToken = default)
        {
            var item = Items.First();
            return Task.FromResult(item);
        }

        /// <summary>
        ///     <inheritdoc />
        /// </summary>
        /// <returns></returns>
        public virtual T FirstOrDefault()
        {
            if (Items == null)
                return default;

            return Items.FirstOrDefault();
        }

        /// <summary>
        ///     <inheritdoc />
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<T> FirstOrDefaultAsync(CancellationToken cancellationToken = default)
        {
            var item = Items.FirstOrDefault();
            return Task.FromResult(item);
        }

        #endregion
    }
}