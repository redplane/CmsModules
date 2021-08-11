namespace DataMagic.Abstractions.Interfaces
{
    public interface IPager
    {
        #region Properties

        /// <summary>
        /// Kind of pager request.
        /// </summary>
        string Kind { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Whether items should be returned.
        /// </summary>
        /// <returns></returns>
        bool ShouldItemsQueried();

        /// <summary>
        /// Whether items should be 
        /// </summary>
        /// <returns></returns>
        bool ShouldItemsCounted();

        /// <summary>
        /// Get the number of records which should be skipped.
        /// </summary>
        /// <returns></returns>
        long GetSkippedRecords();

        /// <summary>
        /// Total records to be taken.
        /// </summary>
        /// <returns></returns>
        long GetTotalRecords();

        #endregion
    }
}