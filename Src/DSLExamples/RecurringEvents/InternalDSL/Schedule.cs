using System;
using Common.Specifications;
using DSLExamples.RecurringEvents.SemanticModel;
using PeriodInYear = DSLExamples.RecurringEvents.SemanticModel.PeriodInYear;

namespace DSLExamples.RecurringEvents.InternalDSL
{
	/// <summary>
	/// A builder class for creating an ISpecification 'DateTime' that can tell you if
	/// a candidate DateTime is in the date ranges specified by the user of the builder.
	/// </summary>
	public class Schedule
	{
		const int FIRST_INDEX = 1;
		const int THIRD_INDEX = 3;

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
		/// A Context Variable to hold the starting month of the Schedule period.
		/// </summary>
		public Month PeriodStart { get; private set; }

		public static Schedule First(DayOfWeek dayOfWeek)
		{
			return CreateDayOfWeekInMonthSpecification(FIRST_INDEX, dayOfWeek);
		}

		public static Schedule Third(DayOfWeek dayOfWeek)
		{
			return CreateDayOfWeekInMonthSpecification(THIRD_INDEX, dayOfWeek);
		}

		static Schedule CreateDayOfWeekInMonthSpecification(int index, DayOfWeek dayOfWeek)
		{
			var spec = new DayOfWeekInAMonthSpecification(index, dayOfWeek);

			return new Schedule(spec);
		}

		public Schedule And(Schedule input)
		{
			if (input == null) throw new ArgumentNullException("input");

			// Note: use Or here instead of And
			Content = Content.Or(input.Content);

			return this;
		}

		public Schedule From(Month periodStart)
		{
			if (periodStart == null) throw new ArgumentNullException("periodStart");

			if(PeriodStart != null)
				throw new InvalidOperationException("The starting month has already been set");

			PeriodStart = periodStart;

			return this;
		}

		public Schedule Until(Month periodEnd)
		{
			if (periodEnd == null) throw new ArgumentNullException("periodEnd");

			if(PeriodStart == null)
				throw new InvalidOperationException("The starting period must be set first");

			var periodInYearSpec = new PeriodInYear(PeriodStart, periodEnd);

			Content = Content.And(periodInYearSpec);

			return this;
		}
	}
}