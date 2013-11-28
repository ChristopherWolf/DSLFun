using System;
using System.Collections.Generic;
using System.Linq;
using Common;

namespace SecuritySystemDSL.SemanticModel
{
	public interface IStateMachine 
	{
		State StartingState { get; }

		void AddResetEvent(Event resetEvent);
		bool IsResetEvent(string eventCode);
	}

	public class StateMachine : IStateMachine
	{
		readonly State _startingState;
		readonly IList<Event> _resetEvents;

		public StateMachine(State startingState)
		{
			if (startingState == null) throw new ArgumentNullException("startingState");

			_startingState = startingState;

			_resetEvents = new List<Event>();
		}

		public State StartingState { get { return _startingState; } }

		public void AddResetEvent(Event resetEvent)
		{
			if (resetEvent == null) throw new ArgumentNullException("resetEvent"); 

			_resetEvents.Add(resetEvent);
		}

		public bool IsResetEvent(string eventCode)
		{
			if (eventCode == null) throw new ArgumentNullException("eventCode");

			return _resetEvents.Any(x => x.Code == eventCode);
		}
	}
}