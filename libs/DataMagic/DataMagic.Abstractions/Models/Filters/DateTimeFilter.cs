using System;
using DataMagic.Abstractions.Enums.Operators;

namespace DataMagic.Abstractions.Models.Filters
{
    public class DateTimeFilter
    {
        #region Properties

        /// <summary>
        ///     Value to filter.
        /// </summary>
        public DateTime? Value { get; }

        /// <summary>
        ///     Operator to apply in the comparison.
        /// </summary>
        public DateTimeComparisonOperators Operator { get; }

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