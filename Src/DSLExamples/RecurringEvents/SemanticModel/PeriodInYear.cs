using System;
using Common.Specifications;

namespace DSLExamples.RecurringEvents.SemanticModel
{
	public class PeriodInYear : ISpecification<DateTime>
	{
		readonly Month _endMonth;
		readonly Month _startMonth;

		public Month StartMonth { get { return _startMonth; } }

		public Month EndMonth { get { return _endMonth; } }

		public PeriodInYear(Month startMonth, Month endMonth)
		{
			_startMonth = startMonth;
			_endMonth = endMonth;
		}

		#region ISpecification<DateTime> Members

		public bool IsSatisfiedBy(DateTime item)
		{
			return item.Month >= StartMonth.Number && item.Month <= EndMonth.Number;
		}

		#endregion
	}
}