using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataMagic.Abstractions.Interfaces;
using DataMagic.Abstractions.Models;
using MongoDB.Driver.Linq;

namespace DataMagic.MongoDatabase.Extensions
{
    public static class SearchResultExtensions
    {
        #region Methods

        /// <summary>
        ///     Do pagination on IQueryable list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="pager"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<ISearchResult<T>> ToSearchResultAsync<T>(this IMongoQueryable<T> source,
            IPager pager, CancellationToken cancellationToken = default)
        {
            // Count total records.
            long totalRecords = 0;
            IEnumerable<T> items = new T[0];

            // Pager is undefined.
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (pager == null)
            {
                items = source.ToArray();
                totalRecords = source.Count();
                return Task.FromResult(new SearchResult<T>(items.ToArray(), totalRecords) as ISearchResult<T>);
            }

            // Whether items should be counted.
            if (pager.ShouldItemsCounted())
                totalRecords = source.Count();

            // Do pagination with one extra item.
            if (pager.ShouldItemsQueried())
                items = source.Skip((int) pager.GetSkippedRecords())
                    .Take((int) pager.GetTotalRecords())
                    .ToArray();

            // Initialize pager result.
            return Task.FromResult(new SearchResult<T>(items, totalRecords) as ISearchResult<T>);
        }

        #endregion
    }
}