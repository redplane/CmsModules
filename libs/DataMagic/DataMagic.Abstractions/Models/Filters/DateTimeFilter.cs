using System;
using DataMagic.Abstractions.Enums;
using DataMagic.Abstractions.Enums.Operators;

namespace DataMagic.Abstractions.Models.Filters
{
	public class DateTimeFilter
	{
		#region Properties

		/// <summary>
		/// Value to filter.
		/// </summary>
		public DateTime? Value { get; private set; }

		/// <summary>
		/// Operator to apply in the comparison.
		/// </summary>
		public DateTimeComparisonOperators Operator { get; private set; }

		#endregion

		#region Constructor

		public DateTimeFilter()
		{

		}

		public DateTimeFilter(DateTime value, DateTimeComparisonOperators @operator)
		{
			Value = value;
			Operator = @operator;
		}

		public DateTimeFilter(string value, DateTimeComparisonOperators @operator)
		{
			Value = DateTime.Parse(value);
			Operator = @operator;
		}

		#endregion
	}
}