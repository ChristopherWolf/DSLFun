namespace SecuritySystemDSL.SemanticModel
{
	public interface ICommandChannel
	{
		void Send(string eventCode);
	}
}