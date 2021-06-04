using DataMagic.Abstractions.Enums;
using DataMagic.Abstractions.Enums.Operators;

namespace DataMagic.Abstractions.Models.Filters
{
	public class DateFilter
	{
		#region Properties

		/// <summary>
		/// Value to filter.
		/// </summary>
		public Date Value { get; private set; }

		/// <summary>
		/// Operator to apply in the comparison.
		/// </summary>
		public DateComparisonOperators Operator { get; private set; }

		#endregion

		#region Constructors

		public DateFilter(Date value, DateComparisonOperators @operator)
		{
			Value = value;
			Operator = @operator;
		}

		#endregion
	}
}