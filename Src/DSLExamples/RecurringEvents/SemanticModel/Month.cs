using System;

namespace DSLExamples.RecurringEvents.SemanticModel
{
	/// <summary>
	/// A Value Object representing the month portion of a date.
	/// </summary>
	public class Month
	{
		const int MAX_MONTH_NUMBER = 12;

		readonly uint _monthNumber;

		public Month(uint monthNumber)
		{
			if(monthNumber > MAX_MONTH_NUMBER)
				throw new ArgumentOutOfRangeException("monthNumber", monthNumber, "The supplied value is not a valid numeric value for a month");

			_monthNumber = monthNumber;
		}

		public uint MonthNumber { get { return _monthNumber; } }

		// ReSharper disable InconsistentNaming
		public static readonly Month January = new Month(1);
		public static readonly Month February = new Month(2);
		public static readonly Month March = new Month(3);
		public static readonly Month April = new Month(4);
		public static readonly Month May = new Month(5);
		public static readonly Month June = new Month(6);
		public static readonly Month July = new Month(7);
		public static readonly Month August = new Month(8);
		public static readonly Month September = new Month(9);
		public static readonly Month October = new Month(10);
		public static readonly Month November = new Month(11);
		public static readonly Month December = new Month(12);
		// ReSharper restore InconsistentNaming
	}
}