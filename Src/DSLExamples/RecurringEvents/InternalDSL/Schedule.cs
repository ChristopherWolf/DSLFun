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

		ISpecification<DateTime> _content;

		public Schedule(ISpecification<DateTime> content)
		{
			if (content == null) throw new ArgumentNullException("content");

			_content = content;
		}

		public ISpecification<DateTime> Content { get { return _content; } }

		public static Schedule First(DayOfWeek dayOfWeek)
		{
			var spec = new DayOfWeekInAMonthSpecification(FIRST_INDEX, dayOfWeek);

			return new Schedule(spec);
		}

		public Schedule And(Schedule input)
		{
			return null;
		}
	}
}