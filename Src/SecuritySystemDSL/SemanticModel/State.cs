using System;
using System.Collections.Generic;
using System.Linq;
using Common;

namespace SecuritySystemDSL.SemanticModel
{
	public interface IState
	{
		string Name { get; }
		void AddTransition(Event trigger, State targetState);
		bool HasTransition(string eventCode);
		IState FindTargetState(string eventCode);
		IEnumerable<IState> GetAllTargets();
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

		public IEnumerable<KeyValuePair<string, Transition>> Transitions { get { return _transitions.Repeat(); } }

		public IEnumerable<Command> Actions { get { return _actions.Repeat(); } }

		public void AddTransition(Event trigger, State targetState)
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

		public IEnumerable<IState> GetAllTargets()
		{
			return _transitions.Values.Select(x => x.Target);
		}

		public void AddAction(Command command)
		{
			if (command == null) throw new ArgumentNullException("command");

			_actions.Add(command);
		}

		public void ExecuteActions(ICommandChannel commandChannel)
		{
			if (commandChannel == null) throw new ArgumentNullException("commandChannel");

			foreach (var action in Actions)
			{
				commandChannel.Send(action.Code);
			}
		}
	}
}