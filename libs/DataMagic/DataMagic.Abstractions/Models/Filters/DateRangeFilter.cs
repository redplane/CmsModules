namespace DataMagic.Abstractions.Models.Filters
{
	public class DateRangeFilter
	{
		#region Properties

		/// <summary>
		/// From when the data will be filtered.
		/// </summary>
		public DateFilter From { get; private set; }

		/// <summary>
		/// To when the data will be filtered.
		/// </summary>
		public DateFilter To { get; private set; }

		#endregion

		#region Constructor

		public DateRangeFilter(DateFilter @from, DateFilter to)
		{
			From = @from;
			To = to;
		}

		#endregion
	}
}