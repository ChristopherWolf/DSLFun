using System;

namespace SecuritySystemDSL.SemanticModel
{
	public class Controller
	{
		readonly IStateMachine _stateMachine;
		readonly ICommandChannel _commandChannel;
		State _currentState;

		public Controller(IStateMachine stateMachine, ICommandChannel commandChannel)
		{
			if (stateMachine == null) throw new ArgumentNullException("stateMachine");
			if (commandChannel == null) throw new ArgumentNullException("commandChannel");

			_stateMachine = stateMachine;
			_commandChannel = commandChannel;

			_currentState = _stateMachine.StartingState;
		}

		public void HandleEventCode(string eventCode)
		{
			if(_currentState.HasTransition(eventCode))
				TransitionTo(_currentState.FindTargetState(eventCode));
			else if(_stateMachine.IsResetEvent(eventCode))
				TransitionTo(_stateMachine.StartingState);

			// Ignore unknown event codes

		}

		void TransitionTo(State target)
		{
//			_currentState = target;
//
//			_currentState.ExecuteActions(_commandChannel);
		}
	}
}