using System;

namespace Common.Specifications
{
	public class PeriodInYear : ISpecification<DateTime>
	{
		readonly int _endMonth;
		readonly int _startMonth;

		public int StartMonth { get { return _startMonth; } }

		public int EndMonth { get { return _endMonth; } }

		public PeriodInYear(int startMonth, int endMonth)
		{
			_startMonth = startMonth;
			_endMonth = endMonth;
		}

		#region ISpecification<DateTime> Members

		public bool IsSatisfiedBy(DateTime item)
		{
//			return item.CompareTo(_start) >= 0 && item.CompareTo(_end) <= 0;

			return item.Month >= StartMonth && item.Month <= EndMonth;
		}

		#endregion
	}
}