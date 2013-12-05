using System;

namespace DSLExamples.RecurringEvents.SemanticModel
{
	public static class SpecificationExtensions
	{
		public static ISpecification<TEntity> And<TEntity>(this ISpecification<TEntity> lhs, ISpecification<TEntity> rhs)
		{
			if (lhs == null) throw new ArgumentNullException("lhs");
			if (rhs == null) throw new ArgumentNullException("rhs");

			return new AndSpecification<TEntity>(lhs, rhs);
		}

		public static ISpecification<TEntity> Or<TEntity>(this ISpecification<TEntity> lhs, ISpecification<TEntity> rhs)
		{
			if (lhs == null) throw new ArgumentNullException("lhs");
			if (rhs == null) throw new ArgumentNullException("rhs");

			return new OrSpecification<TEntity>(lhs, rhs);
		}

		public static ISpecification<TEntity> Not<TEntity>(this ISpecification<TEntity> source)
		{
			if (source == null) throw new ArgumentNullException("source");

			return new NotSpecification<TEntity>(source);
		}
	}
}