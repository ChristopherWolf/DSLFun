namespace Common.Specifications
{
	public interface ISpecification<in TItem>
	{
		bool IsSatisfiedBy(TItem item);
	}
}