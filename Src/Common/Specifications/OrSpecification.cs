using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Specifications
{
	public class OrSpecification<TItem> : ISpecification<TItem>
	{
		readonly ISpecification<TItem>[] _innerSpecifications;

		public IEnumerable<ISpecification<TItem>> InnerSpecifications { get { return _innerSpecifications; } }

		public OrSpecification(IEnumerable<ISpecification<TItem>> innerSpecifications)
			: this(innerSpecifications.ToArray()) {}

		public OrSpecification(params ISpecification<TItem>[] innerSpecifications)
		{
			if (innerSpecifications == null) throw new ArgumentNullException("innerSpecifications");

			_innerSpecifications = innerSpecifications;
		}

		#region ISpecification<TItem> Members

		public bool IsSatisfiedBy(TItem item)
		{
			return InnerSpecifications.Any(s => s.IsSatisfiedBy(item));
		}	

		#endregion
	}
}