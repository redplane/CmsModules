using System;
using DataMagic.Abstractions.Constants;
using DataMagic.Abstractions.Interfaces;

namespace DataMagic.Abstractions.Models.Pagers
{
    public class DefaultPager : IPager
    {
        #region Properties

        /// <summary>
        /// How many records should be skipepd.
        /// </summary>
        private readonly long _skippedRecords;

        /// <summary>
        /// Whether items should be queried or not.
        /// </summary>
        private readonly bool _shouldItemsQueried;

        /// <summary>
        /// Whether items shoudl be counted or not.
        /// </summary>
        private readonly bool _shouldItemsCounted;

        #endregion

        #region Accessors

        public string Kind { get; private set; }

        /// <summary>
        /// Page in which records should be fetched.
        /// </summary>
        public long Page { get; private set; }

        /// <summary>
        /// Number of items to be taken.
        /// </summary>
        public long TotalRecords { get; private set; }

        #endregion

        #region Constructor

        protected DefaultPager()
        {
            Kind = PagerKinds.Default;
        }


        public DefaultPager(long page, long totalRecords) : this()
        {
            if (page < 1)
                throw new ArgumentException("Cannot be smaller than 1", nameof(page));

            if (totalRecords < 0)
                throw new ArgumentException("Cannot smaller than 0", nameof(totalRecords));

            Page = page;
            TotalRecords = totalRecords;

            // Calculate the skipped records.
            _skippedRecords = (Page - 1) * TotalRecords;
        }

        public DefaultPager(long page, long totalRecords, bool shouldItemsQueried = false,
            bool shouldItemsCounted = false) : this(page, totalRecords)
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