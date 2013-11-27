using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;

namespace Common.UnitTests.TestingHelpers
{
	public class AutoFakeItEasyDataAttribute : AutoDataAttribute
	{
		public AutoFakeItEasyDataAttribute()
			: base((IFixture) new Fixture()
				                  .CustomizeWithFakeItEasy())
		{
		}

		public AutoFakeItEasyDataAttribute(params ICustomization[] customizations)
			: base((IFixture) new Fixture()
				                  .CustomizeWithFakeItEasy(customizations))
		{
		}
	}
}