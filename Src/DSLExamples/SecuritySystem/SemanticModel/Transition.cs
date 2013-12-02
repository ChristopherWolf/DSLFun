using System;

namespace DSLExamples.SecuritySystem.SemanticModel
{
	public class Transition
	{
		readonly IState _source;
		readonly Event _trigger;
		readonly IState _target;

		public Transition(IState source, Event trigger, IState target)
		{
			if (source == null) throw new ArgumentNullException("source");
			if (trigger == null) throw new ArgumentNullException("trigger");
			if (target == null) throw new ArgumentNullException("target");

			_source = source;
			_trigger = trigger;
			_target = target;
		}

		public IState Source
		{
			get { return _source; }
		}

		public Event Trigger
		{
			get { return _trigger; }
		}

		public IState Target
		{
			get { return _target; }
		}
	}
}