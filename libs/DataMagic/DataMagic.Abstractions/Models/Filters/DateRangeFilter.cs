namespace DataMagic.Abstractions.Models.Filters
{
    public class DateRangeFilter
    {
        #region Constructor

        public DateRangeFilter(DateFilter from, DateFilter to)
        {
            From = from;
            To = to;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     From when the data will be filtered.
        /// </summary>
        public DateFilter From { get; }

        /// <summary>
        ///     To when the data will be filtered.
        /// </summary>
        public DateFilter To { get; }

        #endregion
    }
}