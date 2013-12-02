namespace DSLExamples.SemanticModel
{
	public interface ICommandChannel
	{
		void Send(string eventCode);
	}
}