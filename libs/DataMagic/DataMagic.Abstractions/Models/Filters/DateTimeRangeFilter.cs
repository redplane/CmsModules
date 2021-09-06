namespace DataMagic.Abstractions.Models.Filters
{
    public class DateTimeRangeFilter
    {
        #region Constructor

        public DateTimeRangeFilter(DateTimeFilter from, DateTimeFilter to)
        {
            From = from;
            To = to;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     From when the items should be filtered.
        /// </summary>
        public DateTimeFilter From { get; }

        /// <summary>
        ///     To when the items should be filtered.
        /// </summary>
        public DateTimeFilter To { get; }

        #endregion
    }
}