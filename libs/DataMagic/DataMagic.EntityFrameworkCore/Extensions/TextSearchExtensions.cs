using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DataMagic.Abstractions.Enums.Operators;
using DataMagic.Abstractions.Models.Filters;
using Microsoft.EntityFrameworkCore;

namespace DataMagic.EntityFrameworkCore.Extensions
{
	public static class TextSearchExtensions
	{
		#region Methods

		/// <summary>
		///     Do search on specific text field base on specific conditions.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="items"></param>
		/// <param name="property"></param>
		/// <param name="filter"></param>
		/// <returns></returns>
		public static IQueryable<T> WithTextSearch<T>(this IQueryable<T> items, Expression<Func<T, string>> property,
			TextFilter filter)
		{
			// Filter is null.
			if (filter == null)
				return items;

			// Property is null.
			if (property == null)
				throw new ArgumentNullException(nameof(property));

			// No text to be filtered.
			if (string.IsNullOrWhiteSpace(filter.Value))
				return items;

			var memberExpression = property.Body as MemberExpression;
			if (memberExpression == null || !(memberExpression.Member is PropertyInfo))
				throw new ArgumentException("Property expected", nameof(property));

			BinaryExpression expression = null;

			// Expression of LIKE.
			var sqlLikeExpression = typeof(DbFunctionsExtensions).GetMethod(nameof(DbFunctionsExtensions.Like),
				BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
				null,
				new[] { typeof(DbFunctions), typeof(string), typeof(string) },
				null)!;

			switch (filter.Operator)
			{
				case TextComparisonOperators.StartWith:
					var startsWithPattern = Expression.Constant($"{filter.Value}%", typeof(string));
					var startWithExpressionCall = Expression
						.Call(sqlLikeExpression!, Expression.Property(null, typeof(EF), nameof(EF.Functions)),
							property.Body, startsWithPattern);

					expression = Expression.OrElse(Expression.Constant(false), startWithExpressionCall);
					break;

				case TextComparisonOperators.EndWith:
					var endsWithPattern = Expression.Constant($"%{filter.Value}", typeof(string));
					var endsWithExpressionCall = Expression
						.Call(sqlLikeExpression!, Expression.Property(null, typeof(EF), nameof(EF.Functions)),
							property.Body, endsWithPattern);

					expression = Expression.OrElse(Expression.Constant(false), endsWithExpressionCall);
					break;

				case TextComparisonOperators.Contains:
					var likePattern = Expression.Constant($"%{filter.Value}%", typeof(string));
					var likeExpressionCall = Expression
						.Call(sqlLikeExpression!, Expression.Property(null, typeof(EF), nameof(EF.Functions)),
							property.Body, likePattern);

					expression = Expression.OrElse(Expression.Constant(false), likeExpressionCall);
					break;

				case TextComparisonOperators.Equal:
					var textExpression = Expression.Constant(filter.Value, typeof(string));
					expression = Expression.Equal(property.Body, textExpression);
					break;

				default:
					var caseInsensitiveTextExpression =
						Expression.Constant(filter.Value?.ToLowerInvariant() ?? string.Empty, typeof(string));
					var methodInfo = typeof(string).GetMethod(nameof(string.ToLower), Type.EmptyTypes)!;

					expression = Expression.Equal(Expression.Call(property.Body, methodInfo!),
						caseInsensitiveTextExpression);
					break;
			}

			var lambda =
				Expression.Lambda<Func<T, bool>>(expression, property.Parameters.Single());
			return items.Where(lambda);
		}
		
		#endregion
	}
}