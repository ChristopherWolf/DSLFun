using System;
using System.Collections.Generic;

namespace DSLExamples.ProgressiveInterfaces
{
	public interface IMessageBuilderPostBuild
	{
		IMessageBuilderPostTo To(string to);
	}

	public interface IMessageBuilderPostTo : IMessageBuilderPostBuild
	{
		IMessageBuilderPostCc Cc(string cc);
		IMessageBuilderPostSubject Subject(string subject);
	}

	public interface IMessageBuilderPostCc
	{
		IMessageBuilderPostCc Cc(string cc);
		IMessageBuilderPostSubject Subject(string subject);
	}

	public interface IMessageBuilderPostSubject
	{
		EmailMessage Body(string body);
	}

	public class EmailMessageBuilder : IMessageBuilderPostBuild, IMessageBuilderPostTo, IMessageBuilderPostCc, IMessageBuilderPostSubject
	{
		string _subject;

		readonly IList<string> _toList;
		readonly IList<string> _ccList;

		EmailMessageBuilder()
		{
			_toList = new List<string>();
			_ccList = new List<string>();
		}

		public static IMessageBuilderPostBuild Build()
		{
			return new EmailMessageBuilder();
		}

		public IMessageBuilderPostTo To(string to)
		{
			_toList.Add(to);

			return this;
		}

		public IMessageBuilderPostCc Cc(string cc)
		{
			_ccList.Add(cc);

			return this;
		}

		public IMessageBuilderPostSubject Subject(string subject)
		{
			_subject = subject;

			return this;
		}

		public EmailMessage Body(string body)
		{
			return new EmailMessage(_toList, _ccList, _subject, body);
		}
	}


	public class BuilderConsumer
	{
		public BuilderConsumer()
		{
			var email = EmailMessageBuilder.Build()
			                               .To("").To("")
			                               .Cc("").Cc("")
			                               .Subject("")
			                               .Body("");

			Console.WriteLine(email);
		}
	}
}