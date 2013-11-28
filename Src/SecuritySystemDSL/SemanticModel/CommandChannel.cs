using System;

namespace SecuritySystemDSL.SemanticModel
{
	public interface ICommandChannel
	{
		void Send(string eventCode);
	}

	public class ConsoleCommandChannel : ICommandChannel
	{
		public void Send(string eventCode)
		{
			if (eventCode == null) throw new ArgumentNullException("eventCode");

			var message = string.Format("Command channel received event code: {0}", eventCode);
			              
			Console.WriteLine(message);
		}
	}
}