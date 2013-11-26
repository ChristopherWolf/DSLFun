using System;

namespace SecuritySystemDSL.SemanticModel
{
	public interface IState
	{
		string Name { get; }
	}

	public class State : IState
	{
		readonly string _name;

		public State(string name)
		{
			if (name == null) throw new ArgumentNullException("name");

			_name = name;
		}

		public string Name
		{
			get { return _name; }
		}
	}
}