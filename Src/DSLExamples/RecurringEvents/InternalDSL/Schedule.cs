using System;
using Common.Specifications;
using DSLExamples.RecurringEvents.SemanticModel;

namespace DSLExamples.RecurringEvents.InternalDSL
{
	/// <summary>
	/// A builder class for creating an ISpecification 'DateTime' that can tell you if
	/// a candidate DateTime is in the date ranges specified by the user of the builder.
	/// </summary>
	public class Schedule
	{
		const int FIRST_INDEX = 1;

		public Schedule(ISpecification<DateTime> content)
		{
			if (content == null) throw new ArgumentNullException("content");

			Content = content;
		}

		/// <summary>
		/// The specification created by this builder.
		/// </summary>
		public ISpecification<DateTime> Content { get; private set; }

		/// <summary>
		/// A Contenxt Variable to hold the starting month of the Schedule period.
		/// </summary>
		public Month StartingMonth { get; private set; }

		public static Schedule First(DayOfWeek dayOfWeek)
		{
			var spec = new DayOfWeekInAMonthSpecification(FIRST_INDEX, dayOfWeek);

			return new Schedule(spec);
		}

		public Schedule And(Schedule input)
		{
			if (input == null) throw new ArgumentNullException("input");

			Content = Content.Or(input.Content);

			return this;
		}

		public Schedule From(Month month)
		{
			if (month == null) throw new ArgumentNullException("month");

			StartingMonth = month;

			return this;
		}
	}
}