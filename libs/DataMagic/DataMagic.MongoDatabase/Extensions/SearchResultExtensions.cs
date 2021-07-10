using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataMagic.Abstractions.Interfaces;
using DataMagic.Abstractions.Models;
using MongoDB.Driver;
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

        /// <summary>
        ///     Find and build projected search result from mongo database.
        /// </summary>
        /// <returns></returns>
        public static async Task<ISearchResult<TProjection>> ToSearchResultAsync<T, TProjection>(
            this IFindFluent<T, TProjection> source,
            IPager pager, CancellationToken cancellationToken = default)
        {
            // Count total records.
            var totalRecords = await source.CountDocumentsAsync(cancellationToken);

            var items = new List<TProjection>();

            // Do pagination with one extra item.
            if (pager != null)
            {
                if (pager.ShouldItemsQueried())
                    items = await source.Skip((int) pager.GetSkippedRecords())
                        .Limit((int) pager.GetTotalRecords())
                        .ToListAsync(cancellationToken);

                return new SearchResult<TProjection>(items.ToArray(), totalRecords);
            }

            items = await source.ToListAsync(cancellationToken);
            return new SearchResult<TProjection>(items.ToArray(), totalRecords);
        }

        /// <summary>
        ///     Find and build projected search result from mongo database.
        /// </summary>
        /// <returns></returns>
        public static async Task<ISearchResult<T>> ToSearchResultAsync<T>(this IAggregateFluent<T> source,
            IPager pager, CancellationToken cancellationToken = default)
        {
            // Count total records.
            var aggregateResult = await source.Count().FirstOrDefaultAsync(cancellationToken);
            var items = new List<T>();

            // Do pagination with one extra item.
            if (pager != null)
            {
                if (pager.ShouldItemsQueried())
                    items = await source
                        .Skip((int) pager.GetSkippedRecords())
                        .Limit((int) pager.GetTotalRecords())
                        .ToListAsync(cancellationToken);

                // Initialize pager result.
                return new SearchResult<T>(items.ToArray(), aggregateResult.Count);
            }

            items = await source.ToListAsync(cancellationToken);
            return new SearchResult<T>(items.ToArray(), aggregateResult.Count);
        }
        #endregion
    }
}