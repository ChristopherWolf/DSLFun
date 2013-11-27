using System;
using System.Collections.Generic;
using System.Linq;

namespace SecuritySystemDSL.SemanticModel
{
	public class StateMachine
	{
		readonly IState _startingState;

		public StateMachine(IState startingState)
		{
			if (startingState == null) throw new ArgumentNullException("startingState");

			_startingState = startingState;
		}

		public IState StartingState
		{
			get { return _startingState; }
		}

		public IEnumerable<IState> AllPossibleStates
		{
			get { return DeterminePossibleStates(); }
		}

		IEnumerable<IState> DeterminePossibleStates()
		{
			var list = new List<IState>();

			

			return list;
		}
	}
}