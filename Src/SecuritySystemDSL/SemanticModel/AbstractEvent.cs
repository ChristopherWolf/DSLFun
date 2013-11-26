﻿using System;

namespace SecuritySystemDSL.SemanticModel
{
	public abstract class AbstractEvent
	{
		readonly string _name;
		readonly string _code;

		protected AbstractEvent(string name, string code)
		{
			if (name == null) throw new ArgumentNullException("name");
			if (code == null) throw new ArgumentNullException("code");

			_name = name;
			_code = code;
		}

		public string Name
		{
			get { return _name; }
		}

		public string Code
		{
			get { return _code; }
		}
	}

	public class Command : AbstractEvent
	{
		public Command(string name, string code) 
			: base(name, code)
		{
		}
	}

	public class Event : AbstractEvent
	{
		public Event(string name, string code)
			: base(name, code)
		{
		}
	}
}