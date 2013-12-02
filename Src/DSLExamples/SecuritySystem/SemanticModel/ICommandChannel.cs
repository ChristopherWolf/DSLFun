namespace DSLExamples.SecuritySystem.SemanticModel
{
	public interface ICommandChannel
	{
		void Send(string eventCode);
	}
}