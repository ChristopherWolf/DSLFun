using System;

namespace Common.Specifications
{
	public class OrSpecification<TItem> : ISpecification<TItem>
	{
		readonly ISpecification<TItem> _lhs;
		readonly ISpecification<TItem> _rhs;

		public ISpecification<TItem> Lhs { get { return _lhs; } }

		public ISpecification<TItem> Rhs { get { return _rhs; } }

		public OrSpecification(ISpecification<TItem> lhs, ISpecification<TItem> rhs)
		{
			if (lhs == null) throw new ArgumentNullException("lhs");
			if (rhs == null) throw new ArgumentNullException("rhs");

			_lhs = lhs;
			_rhs = rhs;
		}

		#region ISpecification<TItem> Members

		public bool IsSatisfiedBy(TItem item)
		{
			return Lhs.IsSatisfiedBy(item) || Rhs.IsSatisfiedBy(item);
		}

		#endregion
	}
}