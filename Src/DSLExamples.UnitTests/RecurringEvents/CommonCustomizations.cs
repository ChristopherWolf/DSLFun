using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common.UnitTests.TestingHelpers;
using DSLExamples.RecurringEvents.SemanticModel;
using Ploeh.AutoFixture;

namespace DSLExamples.UnitTests.RecurringEvents
{
	#region Customizations

	public class ValidMonthCustomization : ICustomization
	{
		public void Customize(IFixture fixture)
		{
			var monthGenerator = fixture.Create<MonthGenerator>();

			// Pick random start month but ensure that there is always at least two months after the start
			// so that start and end are not the same and there is one month between them
			var startMonth = monthGenerator.First(x => x.Number < 11);
			var endMonth = new Month(startMonth.Number + 2);

			fixture.Inject(new MonthTuple(startMonth, endMonth));

			fixture.Register(monthGenerator.First);
		}
	}

	public class ValidMonthAttribute : AutoFakeItEasyDataAttribute
	{
		public ValidMonthAttribute()
			: base(new ValidMonthCustomization())
		{
		}
	}

	public class MonthGenerator : IEnumerable<Month>
	{
		readonly Random _randomizer;

		public MonthGenerator(int seed)
		{
			_randomizer = new Random(seed);
		}

		public IEnumerator<Month> GetEnumerator()
		{
			while (true)
			{
				var monthNumber = _randomizer.Next(1, 12);

				yield return new Month(monthNumber);
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	public class MonthTuple
	{
		readonly Month _startMonth;
		readonly Month _endMonth;

		public MonthTuple(Month startMonth, Month endMonth)
		{
			if (startMonth == null) throw new ArgumentNullException("startMonth");
			if (endMonth == null) throw new ArgumentNullException("endMonth");

			_startMonth = startMonth;
			_endMonth = endMonth;
		}

		public Month StartMonth { get { return _startMonth; } }

		public Month EndMonth { get { return _endMonth; } }
	}

	#endregion
}
