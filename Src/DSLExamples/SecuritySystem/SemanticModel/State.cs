using System;
using System.Collections.Generic;
using Common;
using DSLExamples.SecuritySystem.SemanticModel;

namespace DSLExamples.SecuritySystem.SemanticModel
{
	public interface IState 
	{
		void AddTransition(Event trigger, IState targetState);
		bool HasTransition(string eventCode);

		IState FindTargetState(string eventCode);

		void AddAction(Command command);
		void ExecuteActions(ICommandChannel commandChannel);
	}

	public class State : IState
	{
		readonly string _name;
		readonly IDictionary<string, Transition> _transitions;
		readonly IList<Command> _actions;

		public State(string name)
		{
			if (name == null) throw new ArgumentNullException("name");

			_name = name;

			_transitions = new Dictionary<string, Transition>();
			_actions = new List<Command>();
		}

		public string Name { get { return _name; } }

		public IEnumerable<Transition> Transitions { get { return _transitions.Values.Repeat(); } }

		public void AddTransition(Event trigger, IState targetState)
		{
			if (trigger == null) throw new ArgumentNullException("trigger");
			if (targetState == null) throw new ArgumentNullException("targetState");

			_transitions.Add(trigger.Code, new Transition(this, trigger, targetState));
		}

		public bool HasTransition(string eventCode)
		{
			if (eventCode == null) throw new ArgumentNullException("eventCode");

			return _transitions.ContainsKey(eventCode);
		}

		public IState FindTargetState(string eventCode)
		{
			if (eventCode == null) throw new ArgumentNullException("eventCode");

			var transition = _transitions[eventCode];

			return transition.Target;
		}

		public void AddAction(Command command)
		{
			if (command == null) throw new ArgumentNullException("command");

			_actions.Add(command);
		}

		public void ExecuteActions(ICommandChannel commandChannel)
		{
			if (commandChannel == null) throw new ArgumentNullException("commandChannel");

			foreach (var action in _actions)
			{
				commandChannel.Send(action.Code);
			}
		}

		public override string ToString()
		{
			return Name;
		}
	}
}