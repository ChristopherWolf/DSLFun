using System;

namespace Common.Specifications
{
	public class IsBetweenSpecification<TItem> : ISpecification<TItem> where TItem : IComparable<TItem>
	{
		readonly TItem _start;
		readonly TItem _end;

		public TItem Start { get { return _start; } }

		public TItem End { get { return _end; } }

		public IsBetweenSpecification(TItem start, TItem end)
		{
			_start = start;
			_end = end;
		}

		#region ISpecification<TItem> Members

		public bool IsSatisfiedBy(TItem item)
		{
			return item.CompareTo(_start) >= 0 && item.CompareTo(_end) <= 0;
		}

		#endregion
	}
}