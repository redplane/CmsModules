using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DataMagic.Abstractions.Enums.Operators;
using DataMagic.Abstractions.Models.Filters;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace DataMagic.MongoDatabase.Extensions
{
    public static class DateTimeFilterExtensions
    {
        #region Methods

        public static IMongoQueryable<T> WithDateTimeSearch<T, TValueType>(this IMongoQueryable<T> items,
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
                    expression = Expression.LessThan(left, right);
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

        public static IMongoQueryable<T> WithDateTimeRangeSearch<T, TValueType>(this IMongoQueryable<T> items,
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
        
        public static FilterDefinition<T> WithDateTimeSearch<T, TValueType>(this FilterDefinitionBuilder<T> items,
            Expression<Func<T, TValueType>> property,
            DateTimeFilter dateTimeFilter)
        {
            if (dateTimeFilter == null)
                return FilterDefinition<T>.Empty;

            var memberExpression = property.Body as MemberExpression;
            if (memberExpression == null || !(memberExpression.Member is PropertyInfo))
                throw new ArgumentException("Property expected", nameof(property));

            var left = property.Body;
            Expression right = Expression.Constant(dateTimeFilter.Value, typeof(TValueType));

            BinaryExpression expression = null;

            switch (dateTimeFilter.Operator)
            {
                case DateTimeComparisonOperators.SmallerThan:
                    expression = Expression.LessThan(left, right);
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

        #endregion
    }
}