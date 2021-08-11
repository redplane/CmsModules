using System;
using System.Linq;
using System.Linq.Expressions;
using DataMagic.Abstractions.Enums;

namespace DataMagic.EntityFrameworkCore.Extensions
{
    public static class SortingExtensions
    {
        #region Methods

        /// <summary>
        ///     Do a property sort on a specific property.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="items"></param>
        /// <param name="keySelectorExpression"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static IQueryable<TEntity> WithPropertySort<TEntity, TKey>(this IQueryable<TEntity> items,
            Expression<Func<TEntity, TKey>> keySelectorExpression,
            SortDirections direction = SortDirections.None)
        {
            switch (direction)
            {
                case SortDirections.Ascending:
                    return items.OrderBy(keySelectorExpression);

                case SortDirections.Descending:
                    return items.OrderByDescending(keySelectorExpression);

                default:
                    return items;
            }
        }

        #endregion
    }
}