using System;

namespace DSLExamples.RecurringEvents.SemanticModel
{
	/// <summary>
	/// A Value Object representing the month portion of a date.
	/// </summary>
	public class Month
	{
		const int MIN_MONTH_NUMBER = 1;
		const int MAX_MONTH_NUMBER = 12;

		readonly int _number;

		public Month(int number)
		{
			if (number < MIN_MONTH_NUMBER || number > MAX_MONTH_NUMBER)
				throw new ArgumentOutOfRangeException("number", number, string.Format("The month number must be between {0} and {1}", MIN_MONTH_NUMBER, MAX_MONTH_NUMBER));

			_number = number;
		}

		public int Number { get { return _number; } }

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

		public override string ToString()
		{
			switch (Number)
			{
				case 1:
					return "January";
				case 2:
					return "February";
				case 3:
					return "March";
				case 4:
					return "April";
				case 5:
					return "May";
				case 6:
					return "June";
				case 7:
					return "July";
				case 8:
					return "August";
				case 9:
					return "September";
				case 10:
					return "October";
				case 11:
					return "November";
				case 12:
					return "December";

				default:
					return "Unknown";
			}
		}
	}
}