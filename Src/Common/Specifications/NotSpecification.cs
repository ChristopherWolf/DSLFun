using System;

namespace Common.Specifications
{
	public class NotSpecification<TItem> : ISpecification<TItem>
	{
		readonly ISpecification<TItem> _innerSpecification;

		public ISpecification<TItem> InnerSpecification { get { return _innerSpecification; } }

		public NotSpecification(ISpecification<TItem> innerSpecification)
		{
			if (innerSpecification == null) throw new ArgumentNullException("innerSpecification");

			_innerSpecification = innerSpecification;
		}

		#region ISpecification<TItem> Members

		public bool IsSatisfiedBy(TItem item)
		{
			return !_innerSpecification.IsSatisfiedBy(item);
		}

		#endregion
	}
}