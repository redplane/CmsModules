using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DataMagic.Abstractions.Enums;
using DataMagic.Abstractions.Enums.Operators;
using DataMagic.Abstractions.Models.Filters;

namespace DataMagic.EntityFrameworkCore.Extensions
{
	public static class NumericSearchExtensions
	{
		#region Methods

		/// <summary>
		///     Do search on a numeric field base on specific condition.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TValueType"></typeparam>
		/// <param name="items"></param>
		/// <param name="property"></param>
		/// <param name="range"></param>
		/// <returns></returns>
		public static IQueryable<T> WithNumericSearch<T, TValueType>(this IQueryable<T> items,
			Expression<Func<T, TValueType>> property,
			NumericFilter<TValueType> range)
		{
			if (range == null)
				return items;

			if (property == null)
				return items;

			// Get the value type.
			var valueType = typeof(TValueType);
			var validTypes = NumericFilter<T>.ValidNumericTypes();

			// Check whether the value type is supported.
			var isValueTypeValid = validTypes.Any(x => x == valueType);
			if (!isValueTypeValid)
				throw new Exception($"{nameof(TValueType)} is not supported.");

			var memberExpression = property.Body as MemberExpression;
			if (memberExpression == null || !(memberExpression.Member is PropertyInfo))
				throw new ArgumentException("Property expected", nameof(property));

			var left = property.Body;
			Expression right = Expression.Constant(range.Value, valueType);

			BinaryExpression expression = null;

			switch (range.Operator)
			{
				case NumericComparisonOperators.Equal:
					expression = Expression.Equal(left, right);
					break;

				case NumericComparisonOperators.SmallerThan:
					expression = Expression.LessThan(left, right);
					break;

				case NumericComparisonOperators.SmallerThanEqualTo:
					expression = Expression.LessThanOrEqual(left, right);
					break;

				case NumericComparisonOperators.GreaterThanEqualTo:
					expression = Expression.GreaterThanOrEqual(left, right);

					break;

				case NumericComparisonOperators.GreaterThan:
					expression = Expression.GreaterThan(left, right);
					break;

				default:
					throw new ArgumentException("Not supported comparison mode");
			}

			var lambda =
				Expression.Lambda<Func<T, bool>>(expression, property.Parameters.Single());
			return items.Where(lambda);
		}

		#endregion
	}
}