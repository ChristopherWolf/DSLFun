using System;
using System.Collections.Generic;
using System.Linq;
using Common.UnitTests.TestingHelpers;
using FluentAssertions;
using Ploeh.AutoFixture;
using Xunit.Extensions;

// ReSharper disable CheckNamespace
namespace Common.UnitTests.EnumerableExtensionsTests
// ReSharper restore CheckNamespace
{
    public class WhenTestingTheRepeatMethod
    {
		[Theory, AutoFakeItEasyData]
		public void TheReturnedSequenceShouldBeCorrect(IFixture fixture)
		{
			// Arrange
			var source = fixture.Create<IEnumerable<int>>().ToArray();

			// Act
			var result = source.Repeat();

			// Assert
			result.Should().Equal(source);
		}

		[Theory, AutoFakeItEasyData]
		public void TheReturnedSequenceShouldNotBeCastableBackToTheOriginalType(IFixture fixture)
		{
			// Arrange
			var source = fixture.Create<List<int>>();

			// Act
			Action action = () =>
				{
					var newEnumerable = ((List<int>) source.Repeat());

					newEnumerable.Clear();
				};

			// Assert
			action.ShouldThrow<InvalidCastException>();
		}
    }
}
