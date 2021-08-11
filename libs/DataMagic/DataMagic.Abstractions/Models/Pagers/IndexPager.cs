using System;
using DataMagic.Abstractions.Constants;
using DataMagic.Abstractions.Interfaces;

namespace DataMagic.Abstractions.Models.Pagers
{
    public class IndexPager : IPager
    {
        #region Properties

        /// <summary>
        /// Whether items should be queried or not.
        /// </summary>
        private readonly bool _shouldItemsQueried;

        /// <summary>
        /// Whether items should be counted or not.
        /// </summary>
        private readonly bool _shouldItemsCounted;

        /// <summary>
        /// How many records should be skipped.
        /// </summary>
        private readonly long _skippedRecords;

        #endregion

        #region Accessors

        public string Kind { get; private set; }

        /// <summary>
        /// Index of page to fetch data from.
        /// </summary>
        public long PageIndex { get; private set; }

        /// <summary>
        /// Total records which will be taken on the designated page.
        /// </summary>
        public long TotalRecords { get; private set; }

        #endregion

        #region Constructor

        protected IndexPager()
        {
            Kind = PagerKinds.Index;
        }

        public IndexPager(long pageIndex, long totalRecords) : this()
        {
            PageIndex = pageIndex;
            TotalRecords = totalRecords;

            if (pageIndex < 0)
                throw new ArgumentException("Cannot be smaller than 1", nameof(pageIndex));

            if (totalRecords < 0)
                throw new ArgumentException("Cannot smaller than 0", nameof(totalRecords));

            PageIndex = pageIndex;
            TotalRecords = totalRecords;

            // Calculate the skipped records.
            _skippedRecords = PageIndex * TotalRecords;
        }

        public IndexPager(long pageIndex, long totalRecords, bool shouldItemsQueried = false,
            bool shouldItemsCounted = false) : this(pageIndex, totalRecords)
        {
            _shouldItemsQueried = shouldItemsQueried;
            _shouldItemsCounted = shouldItemsCounted;
        }

        #endregion

        #region Methods

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <returns></returns>
        public virtual bool ShouldItemsQueried()
        {
            if (TotalRecords < 1)
                return false;

            return _shouldItemsQueried;
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <returns></returns>
        public virtual bool ShouldItemsCounted()
        {
            return _shouldItemsCounted;
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <returns></returns>
        public virtual long GetSkippedRecords()
        {
            return _skippedRecords;
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <returns></returns>
        public virtual long GetTotalRecords()
        {
            return TotalRecords;
        }

        #endregion
    }
}