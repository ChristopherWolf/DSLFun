using System;

namespace SecuritySystemDSL.SemanticModel
{
	public class Transition
	{
		readonly IState _startState;
		readonly Event _trigger;
		readonly IState _endState;

		public Transition(IState startState, Event trigger, IState endState)
		{
			if (startState == null) throw new ArgumentNullException("startState");
			if (trigger == null) throw new ArgumentNullException("trigger");
			if (endState == null) throw new ArgumentNullException("endState");

			_startState = startState;
			_trigger = trigger;
			_endState = endState;
		}

		public IState StartState
		{
			get { return _startState; }
		}

		public Event Trigger
		{
			get { return _trigger; }
		}

		public IState EndState
		{
			get { return _endState; }
		}

		public string EventCode
		{
			get { return Trigger.Code; }
		}
	}
}