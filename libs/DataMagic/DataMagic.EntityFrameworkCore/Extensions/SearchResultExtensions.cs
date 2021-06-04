﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataMagic.Abstractions.Interfaces;
using DataMagic.Abstractions.Models;
using Microsoft.EntityFrameworkCore;

namespace DataMagic.EntityFrameworkCore.Extensions
{
	public static class SearchResultExtensions
	{
		#region Methods

		/// <summary>
		/// Do pagination on IQueryable list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="pager"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public static async Task<ISearchResult<T>> ToSearchResultAsync<T>(this IQueryable<T> source,
																					IPager pager, CancellationToken cancellationToken = default)
		{
			// Count total records.
			long totalRecords = 0;

			// Whether items should be counted.
			if (pager.ShouldItemsCounted())
				totalRecords = await source.CountAsync(cancellationToken);

			var items = new List<T>();

			// Pager is undefined.
			// ReSharper disable once ConditionIsAlwaysTrueOrFalse
			if (pager == null)
			{
				items = await source.ToListAsync(cancellationToken);
				return new SearchResult<T>(items.ToArray(), totalRecords);
			}

			// Do pagination with one extra item.
			if (pager.ShouldItemsQueried())
			{
				items = await source.Skip((int)pager.GetSkippedRecords())
					.Take((int)pager.GetTotalRecords())
					.ToListAsync(cancellationToken);
			}

			// Initialize pager result.
			return new SearchResult<T>(items, totalRecords);
		}

		/// <summary>
		/// Do pagination on IEnumerable
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="pager"></param>
		/// <returns></returns>
		public static ISearchResult<T> ToSearchResult<T>(this IEnumerable<T> source,
																 IPager pager)
		{
			if (source == null)
				return null;

			// Count total records.
			IEnumerable<T> enumerable = source as T[] ?? source.ToArray();
			var totalRecords = enumerable.Count();

			// Items list in the search result.
			var items = new T[0];

			// Do pagination with one extra item.
			if (pager != null)
			{
				if (pager.ShouldItemsQueried())
				{
					items = enumerable.Skip((int)pager.GetSkippedRecords())
						.Take((int) pager.GetTotalRecords())
						.ToArray();
				}

				return new SearchResult<T>(items, totalRecords);
			}

			return new SearchResult<T>(items.ToArray(), totalRecords);
		}

		#endregion
	}
}