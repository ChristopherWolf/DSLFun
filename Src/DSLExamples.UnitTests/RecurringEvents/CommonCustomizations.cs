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
			fixture.Register((Generator<int> g) =>
				{
					var monthNumber = g.First(x => x >= 1 && x <= 12);

					return new Month(monthNumber);
				});
		}
	}

	public class ValidMonthAttribute : AutoFakeItEasyDataAttribute
	{
		public ValidMonthAttribute()
			: base(new ValidMonthCustomization())
		{
		}
	}

	#endregion
}
