namespace DSLExamples.RecurringEvents.SemanticModel
{
	public interface ISpecification<in TItem>
	{
		bool IsSatisfiedBy(TItem item);
	}
}