using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DataMagic.Abstractions.Enums;
using DataMagic.Abstractions.Enums.Operators;
using DataMagic.Abstractions.Models.Filters;

namespace DataMagic.EntityFrameworkCore.Extensions
{
	public static class DateTimeSearchExtensions
	{
		#region Methods

		/// <summary>
		///     Do date time search.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="items"></param>
		/// <param name="property"></param>
		/// <param name="filter"></param>
		/// <returns></returns>
		public static IQueryable<T> WithDateTimeSearch<T>(this IQueryable<T> items,
			Expression<Func<T, DateTime?>> property,
			DateTimeFilter filter)
		{
			if (filter == null || property == null)
				return items;

			var memberExpression = property.Body as MemberExpression;
			if (!(memberExpression?.Member is PropertyInfo))
				throw new ArgumentException("Property expected", nameof(property));

			// Value is invalid. No filter is applied.
			if (filter.Value == null)
				return items;

			var left = property.Body;
			Expression right = Expression.Constant(filter.Value, typeof(DateTime?));
			BinaryExpression expression = null;

			switch (filter.Operator)
			{
				case DateTimeComparisonOperators.Equal:
					expression = Expression.Equal(left, right);
					break;

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
					throw new ArgumentException("Not supported operator", nameof(filter.Operator));
			}

			var lambda =
				Expression.Lambda<Func<T, bool>>(expression, property.Parameters.Single());
			return items.Where(lambda);
		}

		/// <summary>
		/// Do date time search
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="items"></param>
		/// <param name="property"></param>
		/// <param name="filter"></param>
		/// <returns></returns>
		public static IQueryable<T> WithDateTimeSearch<T>(this IQueryable<T> items,
			Expression<Func<T, DateTime>> property, DateTimeFilter filter)
		{
			if (filter == null || property == null)
				return items;

			var memberExpression = property.Body as MemberExpression;
			if (!(memberExpression?.Member is PropertyInfo))
				throw new ArgumentException("Property expected", nameof(property));

			// Value is invalid. No filter is applied.
			if (filter.Value == null)
				return items;

			var left = property.Body;
			Expression right = Expression.Constant(filter.Value, typeof(DateTime));
			BinaryExpression expression = null;

			switch (filter.Operator)
			{
				case DateTimeComparisonOperators.Equal:
					expression = Expression.Equal(left, right);
					break;

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
					throw new ArgumentException("Not supported operator", nameof(filter.Operator));
			}

			var lambda =
				Expression.Lambda<Func<T, bool>>(expression, property.Parameters.Single());
			return items.Where(lambda);
		}

		/// <summary>
		/// Do date time search
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="items"></param>
		/// <param name="property"></param>
		/// <param name="filter"></param>
		/// <returns></returns>
		public static IQueryable<T> WithDateTimeRangeSearch<T>(this IQueryable<T> items,
			Expression<Func<T, DateTime>> property, DateTimeRangeFilter filter)
		{
			if (filter == null || property == null)
				return items;

			// Filter the from value.
			items = items.WithDateTimeSearch(property, filter.From);

			// Filter the to value.
			items = items.WithDateTimeSearch(property, filter.To);

			return items;
		}

		/// <summary>
		/// Do date time search
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="items"></param>
		/// <param name="property"></param>
		/// <param name="filter"></param>
		/// <returns></returns>
		public static IQueryable<T> WithDateTimeRangeSearch<T>(this IQueryable<T> items,
			Expression<Func<T, DateTime?>> property, DateTimeRangeFilter filter)
		{
			if (filter == null || property == null)
				return items;

			// Filter the from value.
			items = items.WithDateTimeSearch(property, filter?.From);

			// Filter the to value.
			items = items.WithDateTimeSearch(property, filter?.To);

			return items;
		}

		#endregion
	}
}