namespace DataMagic.Abstractions.Models.Filters
{
	public class DateTimeRangeFilter
	{
		#region Properties

		/// <summary>
		/// From when the items should be filtered.
		/// </summary>
		public DateTimeFilter From { get; private set; }

		/// <summary>
		/// To when the items should be filtered.
		/// </summary>
		public DateTimeFilter To { get; private set; }

		#endregion

		#region Constructor

		public DateTimeRangeFilter(DateTimeFilter @from, DateTimeFilter to)
		{
			From = @from;
			To = to;
		}

		#endregion
	}
}