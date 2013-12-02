using System;
using System.Collections.Generic;
using Common;
using DSLExamples.SemanticModel;

namespace DSLExamples.UnitTests.IntegrationTests
{
	public class HistoryRecordingCommandChannel : ICommandChannel
	{
		readonly IList<string> _eventCodeHistory;

		public IEnumerable<string> EventCodeHistory { get { return _eventCodeHistory.Repeat(); } }

		public HistoryRecordingCommandChannel()
		{
			_eventCodeHistory = new List<string>();
		}

		#region ICommandChannel Members

		public void Send(string eventCode)
		{
			if (eventCode == null) throw new ArgumentNullException("eventCode");

			string message = string.Format("Command channel received event code: {0}", eventCode);

			Console.WriteLine(message);

			_eventCodeHistory.Add(eventCode);
		}

		#endregion
	}
}