using System;

namespace DSLExamples.RecurringEvents.SemanticModel
{
	public class OrSpecification<TItem> : ISpecification<TItem>
	{
		readonly ISpecification<TItem> _lhs;
		readonly ISpecification<TItem> _rhs;

		public ISpecification<TItem> LHS { get { return _lhs; } }

		public ISpecification<TItem> RHS { get { return _rhs; } }

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
			return LHS.IsSatisfiedBy(item) || RHS.IsSatisfiedBy(item);
		}

		#endregion
	}
}