using System;

namespace SecuritySystemDSL.SemanticModel
{
	public class Controller
	{
		readonly StateMachine _stateMachine;
		readonly ICommandChannel _commandChannel;

		public Controller(StateMachine stateMachine, ICommandChannel commandChannel)
		{
			if (stateMachine == null) throw new ArgumentNullException("stateMachine");
			if (commandChannel == null) throw new ArgumentNullException("commandChannel");

			_stateMachine = stateMachine;
			_commandChannel = commandChannel;
		}

		public void HandleEventCode(string eventCode)
		{
			
		}
	}
}