using System;
using System.Linq;

namespace Common.Specifications
{
	public class OrSpecification<TItem> : ISpecification<TItem>
	{
		readonly ISpecification<TItem>[] _innerSpecifications;

		public OrSpecification(params ISpecification<TItem>[] innerSpecifications)
		{
			if (innerSpecifications == null) throw new ArgumentNullException("innerSpecifications");

			_innerSpecifications = innerSpecifications;
		}

		public bool IsSatisfiedBy(TItem item)
		{
			return _innerSpecifications.Any(s => s.IsSatisfiedBy(item));
		}
	}
}