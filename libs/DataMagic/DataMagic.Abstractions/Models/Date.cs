using System;

namespace DataMagic.Abstractions.Models
{
	/// <summary>
	/// Date object which is used for retrieving information from front-end
	/// </summary>
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
	public class Date
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
	{
        #region Properties

        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
        public int Year { get; private set; }

        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
        public int Month { get; private set; }

        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
        public int Day { get; private set; }

        #endregion

        #region Constructor

        public Date(int year, int month, int day)
        {
            Year = year;
            Month = month;
            Day = day;

            // ReSharper disable once VirtualMemberCallInConstructor
            if (!IsValidYear(year))
                throw new Exception("Invalid year");

            // ReSharper disable once VirtualMemberCallInConstructor
            if (!IsValidMonth(month))
                throw new Exception("Invalid month");

            // ReSharper disable once VirtualMemberCallInConstructor
            if (!IsValidDate(year, month, day))
                throw new Exception("Invalid date");
        }

        protected Date()
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Convert to date time with unspecified kind.
        /// </summary>
        /// <returns></returns>
        public DateTime ToDateTime()
        {
            var dateTime = new DateTime(Year, Month, Day, 0, 0, 0, DateTimeKind.Unspecified);
            return dateTime;
        }

        /// <summary>
        /// Convert to date time with specific hour
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public DateTime ToDateTime(int hour, int minute, int seconds)
        {
            if (hour < 0 || hour > 23)
                throw new Exception("Invalid hour");

            if (minute < 0 || minute > 59)
                throw new Exception("Invalid minute");

            if (seconds < 0 || seconds > 59)
                throw new Exception("Invalid second");

            var dateTime = new DateTime(Year, Month, Day, hour, minute, seconds);
            return dateTime;
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToDateTime().ToShortDateString();
        }

#pragma warning disable 659
        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
#pragma warning restore 659
        {
	        if (obj == null || !(obj is Date systemDate))
		        return false;

	        return systemDate.Year == Year && systemDate.Month == Month && systemDate.Day == Day;
        }

        #endregion

        #region Internal methods

        /// <summary>
        /// Whether value of year is valid or not.
        /// </summary>
        /// <returns></returns>
        protected virtual bool IsValidYear(int year)
        {
            // Due to this tutorial: https://docs.microsoft.com/en-us/dotnet/api/system.data.sqltypes.sqldatetime.minvalue?view=net-5.0
            // The minimum year must be from 1753
            return year >= 1753;
        }

        /// <summary>
        /// Whether month is valid or not.
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        protected virtual bool IsValidMonth(int month)
        {
            return (0 < month && month < 13);
        }

        /// <summary>
        /// Whether date is valid or not.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        protected virtual bool IsValidDate(int year, int month, int date)
        {
            if (date < 1)
                return false;

            switch (month)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    return date <= 31;

                case 2:

                    // Year is a leap one.
                    if (year % 4 == 0)
                        return date <= 29;

                    return date <= 28;

                default:
                    return date <= 30;
            }
        }

        /// <summary>
        /// Compare with another date object
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        protected virtual bool Equals(Date other)
        {
	        return Year == other.Year && Month == other.Month && Day == other.Day;
        }

        #endregion
    }
}