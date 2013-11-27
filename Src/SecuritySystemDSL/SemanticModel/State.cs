using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SecuritySystemDSL.SemanticModel
{
	public interface IState
	{
		string Name { get; }

		IEnumerable<KeyValuePair<string, Transition>> Transitions { get; }
	}

	public class State : IState
	{
		readonly string _name;
		readonly IDictionary<string, Transition> _transitions;

		public State(string name)
		{
			if (name == null) throw new ArgumentNullException("name");

			_name = name;

			_transitions = new Dictionary<string, Transition>();
		}

		public string Name
		{
			get { return _name; }
		}

		public IEnumerable<KeyValuePair<string, Transition>> Transitions { get { return _transitions; } }

		public void AddTransition(Event @event, State targetState)
		{
			if (@event == null) throw new ArgumentNullException("@event");
			if (targetState == null) throw new ArgumentNullException("targetState");

		}
	}
}