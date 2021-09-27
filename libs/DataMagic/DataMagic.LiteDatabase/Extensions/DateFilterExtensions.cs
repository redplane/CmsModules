using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DataMagic.Abstractions.Enums.Operators;
using DataMagic.Abstractions.Models.Filters;
using LiteDB;

namespace DataMagic.LiteDatabase.Extensions
{
    public static class DateFilterExtensions
    {
        #region Methods

        public static ILiteQueryable<T> WithDateSearch<T, TValueType>(this ILiteQueryable<T> items,
            Expression<Func<T, TValueType>> property,
            DateFilter dateFilter)
        {
            if (dateFilter == null)
                return items;

            var memberExpression = property.Body as MemberExpression;
            if (memberExpression == null || !(memberExpression.Member is PropertyInfo))
                throw new ArgumentException("Property expected", nameof(property));

            var left = property.Body;
            Expression right = Expression.Constant(dateFilter.Value, typeof(DateTime?));
            BinaryExpression expression = null;

            switch (dateFilter.Operator)
            {
                case DateComparisonOperators.SmallerThan:
                    expression = Expression.Equal(left, right);
                    break;

                case DateComparisonOperators.SmallerThanEqualTo:
                    expression = Expression.LessThanOrEqual(left, right);
                    break;

                case DateComparisonOperators.GreaterThanEqualTo:
                    expression = Expression.GreaterThanOrEqual(left, right);
                    break;

                case DateComparisonOperators.GreaterThan:
                    expression = Expression.GreaterThan(left, right);
                    break;

                default:
                    expression = Expression.Equal(left, right);
                    break;
            }

            var lambda =
                Expression.Lambda<Func<T, bool>>(expression, property.Parameters.Single());
            return items.Where(lambda);
        }

        public static ILiteQueryable<T> WithDateRangeSearch<T, TValueType>(this ILiteQueryable<T> items,
            Expression<Func<T, TValueType>> property,
            DateRangeFilter dateRangeFilter)
        {
            if (dateRangeFilter == null)
                return items;

            if (dateRangeFilter.From != null)
                items = items.WithDateSearch(property, dateRangeFilter.From);

            if (dateRangeFilter.To != null)
                items = items.WithDateSearch(property, dateRangeFilter.To);

            return items;
        }

        #endregion
    }
}