using System;

namespace SecuritySystemDSL.SemanticModel
{
	public class Transition
	{
		readonly State _source;
		readonly Event _trigger;
		readonly State _target;

		public Transition(State source, Event trigger, State target)
		{
			if (source == null) throw new ArgumentNullException("source");
			if (trigger == null) throw new ArgumentNullException("trigger");
			if (target == null) throw new ArgumentNullException("target");

			_source = source;
			_trigger = trigger;
			_target = target;
		}

		public State Source
		{
			get { return _source; }
		}

		public Event Trigger
		{
			get { return _trigger; }
		}

		public State Target
		{
			get { return _target; }
		}

		public string EventCode
		{
			get { return Trigger.Code; }
		}
	}
}