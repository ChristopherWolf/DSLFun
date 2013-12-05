using System;
using System.Collections.Generic;

namespace DSLExamples.ProgressiveInterfaces
{
	public class EmailMessage
	{
		readonly IEnumerable<string> _toList;
		readonly IEnumerable<string> _ccList;
		readonly string _subject;
		readonly string _body;

		public EmailMessage(IEnumerable<string> toList, IEnumerable<string> ccList, string subject, string body)
		{
			if (toList == null) throw new ArgumentNullException("toList");
			if (subject == null) throw new ArgumentNullException("subject");
			if (body == null) throw new ArgumentNullException("body");
			if (ccList == null) throw new ArgumentNullException("ccList");

			_toList = toList;
			_subject = subject;
			_body = body;
			_ccList = ccList;
		}

		public IEnumerable<string> ToList { get { return _toList; } }

		public IEnumerable<string> CcList { get { return _ccList; } }

		public string Subject { get { return _subject; } }

		public string Body { get { return _body; } }
	}
}