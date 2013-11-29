using System;
using System.Linq;

namespace Common.Specifications
{
	public class AndSpecification<TItem> : ISpecification<TItem>
	{
		readonly ISpecification<TItem>[] _innerSpecifications;

		public AndSpecification(params ISpecification<TItem>[] innerSpecifications)
		{
			if (innerSpecifications == null) throw new ArgumentNullException("innerSpecifications");

			_innerSpecifications = innerSpecifications;
		}

		public bool IsSatisfiedBy(TItem item)
		{
			return _innerSpecifications.All(s => s.IsSatisfiedBy(item));
		}
	}
}