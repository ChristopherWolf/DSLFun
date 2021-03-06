﻿using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;

namespace Common.UnitTests.TestingHelpers
{
	public class AutoFakeItEasyDataAttribute : AutoDataAttribute
	{
		public AutoFakeItEasyDataAttribute()
			: base(new Fixture()
						.CustomizeWithFakeItEasy())
		{
		}

		public AutoFakeItEasyDataAttribute(params ICustomization[] customizations)
			: base(new Fixture()
				        .CustomizeWithFakeItEasy(customizations))
		{
		}
	}
}