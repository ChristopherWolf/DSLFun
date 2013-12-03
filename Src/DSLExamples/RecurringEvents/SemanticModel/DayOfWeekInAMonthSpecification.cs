using System;
using System.Collections.Generic;
using System.Linq;
using Common.Specifications;

namespace DSLExamples.RecurringEvents.SemanticModel
{
	/// <summary>
	/// A specification that checks if the DateTime passed to IsSatisfiedBy() is the Nth DayOfTheWeek.
	/// For example, you can test if a DatTime value is the first Monday of a month.
	/// </summary>
	public class DayOfWeekInAMonthSpecification : ISpecification<DateTime>
	{
		readonly int _index;
		readonly DayOfWeek _dayOfWeek;

		/// <summary>
		/// Create a new DayOfWeekInAMonthSpecification.
		/// </summary>
		/// <param name="index">The Nth instance of DayOfWeek to match for. This index starts at 1 for the first match.</param>
		/// <param name="dayOfWeek">The day of the week to mathc for.</param>
		public DayOfWeekInAMonthSpecification(int index, DayOfWeek dayOfWeek)
		{
			if(index <= 0)
				throw new ArgumentOutOfRangeException("index", "The index must be 1 or greater");

			_index = index;
			_dayOfWeek = dayOfWeek;
		}

		public int Index { get { return _index; } }

		public DayOfWeek DayOfWeek { get { return _dayOfWeek; } }

		public bool IsSatisfiedBy(DateTime item)
		{
			var datesMatchingDayOfWeek = GetAllDaysInMonth(item)
											.Where(x => x.DayOfWeek == _dayOfWeek)
											.Select((dateTime, i) => new { DateOnly = dateTime.Date, OneBasedIndex = i + 1 });

			// Compare date portions of DateTime ignoring time
			return datesMatchingDayOfWeek.Any(x => x.DateOnly.Equals(item.Date) && x.OneBasedIndex == _index);
		}

		IEnumerable<DateTime> GetAllDaysInMonth(DateTime date)
		{
			return new DaysInMonthEnumerator(date.Month, date.Year);
		}
	}
}