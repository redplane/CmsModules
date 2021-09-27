using System;
using DataMagic.Abstractions.Constants;
using DataMagic.Abstractions.Interfaces;

namespace DataMagic.Abstractions.Models.Pagers
{
    public class SkipLimitPager : IPager
    {
        #region Properties

        /// <summary>
        ///     Whether items should be queried or not.
        /// </summary>
        private readonly bool _shouldItemsQueried;

        /// <summary>
        ///     Whether items should be counted or not.
        /// </summary>
        private readonly bool _shouldItemsCounted;

        #endregion

        #region Accessors

        public string Kind { get; }

        /// <summary>
        ///     Hos many records should be skipped.
        /// </summary>
        public long SkippedRecords { get; }

        /// <summary>
        ///     The number of records to be taken.
        /// </summary>
        public long TotalRecords { get; }

        #endregion

        #region Constructor

        protected SkipLimitPager()
        {
            Kind = PagerKinds.SkipLimit;
        }

        public SkipLimitPager(long skippedRecords, long totalRecords) : this()
        {
            if (skippedRecords < 0)
                throw new ArgumentException("Cannot be smaller than 0", nameof(skippedRecords));

            if (totalRecords < 0)
                throw new ArgumentException("Cannot be smaller than 0", nameof(totalRecords));

            _shouldItemsQueried = true;
            _shouldItemsCounted = true;

            SkippedRecords = skippedRecords;
            TotalRecords = totalRecords;
        }

        public SkipLimitPager(long skippedRecords, long totalRecords, bool shouldItemsQueried, bool shouldItemsCounted)
            : this(skippedRecords, totalRecords)
        {
            _shouldItemsQueried = shouldItemsQueried;
            _shouldItemsCounted = shouldItemsCounted;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     <inheritdoc />
        /// </summary>
        /// <returns></returns>
        public virtual bool ShouldItemsQueried()
        {
            if (TotalRecords < 1)
                return false;

            return _shouldItemsQueried;
        }

        /// <summary>
        ///     <inheritdoc />
        /// </summary>
        /// <returns></returns>
        public virtual bool ShouldItemsCounted()
        {
            return _shouldItemsCounted;
        }

        /// <summary>
        ///     <inheritdoc />
        /// </summary>
        /// <returns></returns>
        public virtual long GetSkippedRecords()
        {
            return SkippedRecords;
        }

        /// <summary>
        ///     <inheritdoc />
        /// </summary>
        /// <returns></returns>
        public virtual long GetTotalRecords()
        {
            return TotalRecords;
        }

        #endregion
    }
}