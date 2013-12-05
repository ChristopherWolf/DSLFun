using System;
using System.Linq;
using Common.UnitTests.TestingHelpers;
using DSLExamples.RecurringEvents.SemanticModel;
using FluentAssertions;
using Ploeh.AutoFixture;
using Xunit;
using Xunit.Extensions;

// ReSharper disable CheckNamespace
namespace DSLExamples.UnitTests.RecurringEvents.SemanticModel.MonthTests
// ReSharper restore CheckNamespace
{
	public class WhenVerifyingArchitecturalConstraints
	{
		[Theory, AutoFakeItEasyData]
		public void MonthNumberShouldBeExposedCorrectly(Generator<int> generator)
		{
			// Arrange

			// Ensure the month number is between 1 and 12 inclusive
			var expected = generator.First(x => x >= 1 && x <= 12);

			var sut = new Month(expected);

			// Act
			var result = sut.Number;

			// Assert
			result.Should().Be(expected);
		}

		[Theory, AutoFakeItEasyData]
		public void TheConstructorShouldThrowAnExceptionIfThePassedInMonthNumberIsGreaterThan12(Generator<int> generator)
		{
			// Arrange
			var badMonthNumber = generator.First(x => x > 12);

			// Act
			Action action = () => new Month(badMonthNumber);

			// Assert
			action.ShouldThrow<ArgumentOutOfRangeException>();
		}

		[Fact]
		public void TheConstructorShouldThrowAnExceptionIfThePassedInMonthNumberIsLessThan1()
		{
			// Arrange
			const int badMonthNumber = -1;

			// Act
			Action action = () => new Month(badMonthNumber);

			// Assert
			action.ShouldThrow<ArgumentOutOfRangeException>();
		}

		[Fact]
		public void JanuaryShouldHaveTheCorrectMonthNumber()
		{
			// Assert
			Month.January.Number.Should().Be(1);
		}

		[Fact]
		public void FebruaryShouldHaveTheCorrectMonthNumber()
		{
			// Assert
			Month.February.Number.Should().Be(2);
		}

		[Fact]
		public void MarchShouldHaveTheCorrectMonthNumber()
		{
			// Assert
			Month.March.Number.Should().Be(3);
		}

		[Fact]
		public void AprilShouldHaveTheCorrectMonthNumber()
		{
			// Assert
			Month.April.Number.Should().Be(4);
		}

		[Fact]
		public void MayShouldHaveTheCorrectMonthNumber()
		{
			// Assert
			Month.May.Number.Should().Be(5);
		}

		[Fact]
		public void JuneShouldHaveTheCorrectMonthNumber()
		{
			// Assert
			Month.June.Number.Should().Be(6);
		}

		[Fact]
		public void JulyShouldHaveTheCorrectMonthNumber()
		{
			// Assert
			Month.July.Number.Should().Be(7);
		}

		[Fact]
		public void AugustShouldHaveTheCorrectMonthNumber()
		{
			// Assert
			Month.August.Number.Should().Be(8);
		}

		[Fact]
		public void SeptemberShouldHaveTheCorrectMonthNumber()
		{
			// Assert
			Month.September.Number.Should().Be(9);
		}

		[Fact]
		public void OctoberShouldHaveTheCorrectMonthNumber()
		{
			// Assert
			Month.October.Number.Should().Be(10);
		}

		[Fact]
		public void NovemberShouldHaveTheCorrectMonthNumber()
		{
			// Assert
			Month.November.Number.Should().Be(11);
		}

		[Fact]
		public void DecemberShouldHaveTheCorrectMonthNumber()
		{
			// Assert
			Month.December.Number.Should().Be(12);
		}
	}
}