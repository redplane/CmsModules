using DataMagic.Abstractions.Enums.Operators;

namespace DataMagic.Abstractions.Models.Filters
{
    public class DateFilter
    {
        #region Constructors

        public DateFilter(Date value, DateComparisonOperators @operator)
        {
            Value = value;
            Operator = @operator;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Value to filter.
        /// </summary>
        public Date Value { get; }

        /// <summary>
        ///     Operator to apply in the comparison.
        /// </summary>
        public DateComparisonOperators Operator { get; }

        #endregion
    }
}