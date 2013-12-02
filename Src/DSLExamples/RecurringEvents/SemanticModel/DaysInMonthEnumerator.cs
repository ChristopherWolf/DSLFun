using System;
using System.Collections;
using System.Collections.Generic;

namespace DSLExamples.RecurringEvents.SemanticModel
{
	/// <summary>
	/// An IEnumerable 'DateTime' that begins on the first day of the passed in month and year
	/// and returns a sequence containing every day in that month until the next month.
	/// </summary>
	public class DaysInMonthEnumerator : IEnumerable<DateTime>
	{
		readonly DateTime _start;
		readonly DateTime _end;

		public DaysInMonthEnumerator(uint month, uint year)
			: this((int)month, (int)year)
		{
		}

		DaysInMonthEnumerator(int month, int year)
		{
			_start = new DateTime(year, month, 1);
			_end = _start.AddMonths(1);
		}

		public IEnumerator<DateTime> GetEnumerator()
		{
			var current = _start;

			while (current < _end)
			{
				yield return current;
				current = current.AddDays(1);
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}