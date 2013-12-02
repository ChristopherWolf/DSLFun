using System;

namespace Common.Specifications
{
	public class IsBetweenSpecification<TItem> : ISpecification<TItem> where TItem : IComparable<TItem>
	{
		readonly TItem _start;
		readonly TItem _end;

		public IsBetweenSpecification(TItem start, TItem end)
		{
			_start = start;
			_end = end;
		}

		public bool IsSatisfiedBy(TItem item)
		{
			return item.CompareTo(_start) >= 0 && item.CompareTo(_end) <= 0;
		}
	}
}