namespace DataMagic.Abstractions.Models.Filters
{
    public class NumericRangeFilter<TFrom, TTo>
    {
        #region Constructor

        public NumericRangeFilter(NumericFilter<TFrom> from, NumericFilter<TTo> to)
        {
            From = from;
            To = to;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     From which value the comparison is done.
        /// </summary>
        public NumericFilter<TFrom> From { get; }

        /// <summary>
        ///     To which value the comparison is done.
        /// </summary>
        public NumericFilter<TTo> To { get; }

        #endregion
    }
}