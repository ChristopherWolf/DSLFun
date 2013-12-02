using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Specifications
{
	public class AndSpecification<TItem> : ISpecification<TItem>
	{
		readonly ISpecification<TItem>[] _innerSpecifications;

		public IEnumerable<ISpecification<TItem>> InnerSpecifications
		{
			get { return _innerSpecifications; }
		}

		public AndSpecification(IEnumerable<ISpecification<TItem>> innerSpecifications)
			: this(innerSpecifications.ToArray())
		{
		}

		public AndSpecification(params ISpecification<TItem>[] innerSpecifications)
		{
			if (innerSpecifications == null) throw new ArgumentNullException("innerSpecifications");

			_innerSpecifications = innerSpecifications;
		}

		public bool IsSatisfiedBy(TItem item)
		{
			return InnerSpecifications.All(s => s.IsSatisfiedBy(item));
		}
	}
}