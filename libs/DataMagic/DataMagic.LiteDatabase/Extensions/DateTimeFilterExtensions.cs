using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DataMagic.Abstractions.Enums.Operators;
using DataMagic.Abstractions.Models.Filters;
using LiteDB;

namespace DataMagic.LiteDatabase.Extensions
{
    public static class DateTimeFilterExtensions
    {
        #region Methods

        public static ILiteQueryable<T> WithDateTimeSearch<T, TValueType>(this ILiteQueryable<T> items,
            Expression<Func<T, TValueType>> property,
            DateTimeFilter dateTimeFilter)
        {
            if (dateTimeFilter == null)
                return items;

            var memberExpression = property.Body as MemberExpression;
            if (memberExpression == null || !(memberExpression.Member is PropertyInfo))
                throw new ArgumentException("Property expected", nameof(property));

            var left = property.Body;
            Expression right = Expression.Constant(dateTimeFilter.Value, typeof(TValueType));

            BinaryExpression expression = null;

            switch (dateTimeFilter.Operator)
            {
                case DateTimeComparisonOperators.SmallerThan:
                    expression = Expression.Equal(left, right);
                    break;

                case DateTimeComparisonOperators.SmallerThanEqualTo:
                    expression = Expression.LessThanOrEqual(left, right);
                    break;

                case DateTimeComparisonOperators.GreaterThanEqualTo:
                    expression = Expression.GreaterThanOrEqual(left, right);
                    break;

                case DateTimeComparisonOperators.GreaterThan:
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

        public static ILiteQueryable<T> WithDateTimeRangeSearch<T, TValueType>(this ILiteQueryable<T> items,
            Expression<Func<T, TValueType>> property,
            DateTimeRangeFilter dateTimeRangeFilter)
        {
            if (dateTimeRangeFilter == null)
                return items;

            if (dateTimeRangeFilter.From != null)
                items = items.WithDateTimeSearch(property, dateTimeRangeFilter.From);

            if (dateTimeRangeFilter.To != null)
                items = items.WithDateTimeSearch(property, dateTimeRangeFilter.To);

            return items;
        }

        #endregion
    }
}