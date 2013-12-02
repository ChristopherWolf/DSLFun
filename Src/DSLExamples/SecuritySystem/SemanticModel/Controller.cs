using System;

namespace DSLExamples.SecuritySystem.SemanticModel
{
	public class Controller
	{
		readonly IStateMachine _stateMachine;
		readonly ICommandChannel _commandChannel;

		public Controller(IStateMachine stateMachine, ICommandChannel commandChannel)
		{
			if (stateMachine == null) throw new ArgumentNullException("stateMachine");
			if (commandChannel == null) throw new ArgumentNullException("commandChannel");

			_stateMachine = stateMachine;
			_commandChannel = commandChannel;

			CurrentState = _stateMachine.StartingState;
		}

		public IState CurrentState { get; private set; }

		public void HandleEventCode(string eventCode)
		{
			if (eventCode == null) throw new ArgumentNullException("eventCode");

			if (CurrentState.HasTransition(eventCode))
			{
				var newState = CurrentState.FindTargetState(eventCode);

				TransitionTo(newState);
			}
			else if (_stateMachine.IsResetEvent(eventCode))
			{
				TransitionTo(_stateMachine.StartingState);
			}

			// Ignore unknown event codes
		}

		void TransitionTo(IState target)
		{
			CurrentState = target;

			CurrentState.ExecuteActions(_commandChannel);
		}
	}
}