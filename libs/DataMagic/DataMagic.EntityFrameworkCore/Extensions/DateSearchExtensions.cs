using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DataMagic.Abstractions.Enums.Operators;
using DataMagic.Abstractions.Models.Filters;

namespace DataMagic.EntityFrameworkCore.Extensions
{
	public static class DateSearchExtensions
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
		public static IQueryable<T> WithDateSearch<T>(this IQueryable<T> items,
			Expression<Func<T, DateTime?>> property,
			DateFilter filter)
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
			Expression right = Expression.Constant(filter.Value.ToDateTime(), typeof(DateTime?));
			BinaryExpression expression = null;

			switch (filter.Operator)
			{
				case DateComparisonOperators.Equal:
					expression = Expression.Equal(left, right);
					break;

				case DateComparisonOperators.SmallerThan:
					expression = Expression.LessThan(left, right);
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
					throw new ArgumentException("Not supported operator", nameof(filter.Operator));
			}

			var lambda =
				Expression.Lambda<Func<T, bool>>(expression, property.Parameters.Single());
			return items.Where(lambda);
		}

		/// <summary>
		///     Do date time search
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="items"></param>
		/// <param name="property"></param>
		/// <param name="filter"></param>
		/// <returns></returns>
		public static IQueryable<T> WithDateSearch<T>(this IQueryable<T> items,
			Expression<Func<T, DateTime>> property, DateFilter filter)
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
			Expression right = Expression.Constant(filter.Value.ToDateTime(), typeof(DateTime));
			BinaryExpression expression = null;

			switch (filter.Operator)
			{
				case DateComparisonOperators.Equal:
					expression = Expression.Equal(left, right);
					break;

				case DateComparisonOperators.SmallerThan:
					expression = Expression.LessThan(left, right);
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
					throw new ArgumentException("Not supported operator", nameof(filter.Operator));
			}

			var lambda =
				Expression.Lambda<Func<T, bool>>(expression, property.Parameters.Single());
			return items.Where(lambda);
		}

		/// <summary>
		///     Do date time search
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="items"></param>
		/// <param name="property"></param>
		/// <param name="filter"></param>
		/// <returns></returns>
		public static IQueryable<T> WithDateRangeSearch<T>(this IQueryable<T> items,
			Expression<Func<T, DateTime>> property, DateRangeFilter filter)
		{
			if (filter == null || property == null)
				return items;

			// Filter by from value.
			items = items.WithDateSearch(property, filter?.From);

			// Filter by to value.
			items = items.WithDateSearch(property, filter?.To);

			return items;
		}

		#endregion
	}
}