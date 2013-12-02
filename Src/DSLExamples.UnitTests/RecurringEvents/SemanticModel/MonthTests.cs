using System;
using System.Linq;
using Common.UnitTests.TestingHelpers;
using DSLExamples.RecurringEvents.SemanticModel;
using FluentAssertions;
using Ploeh.AutoFixture;
using Ploeh.SemanticComparison.Fluent;
using Xunit;
using Xunit.Extensions;

// ReSharper disable CheckNamespace
namespace DSLExamples.UnitTests.RecurringEvents.SemanticModel.MonthTests
// ReSharper restore CheckNamespace
{
	public class WhenVerifyingArchitecturalConstraints
	{
		[Theory, AutoFakeItEasyData]
		public void MonthNumberShouldBeExposedCorrectly(Generator<uint> generator)
		{
			// Arrange

			// Ensure the month number is between 1 and 12 inclusive
			var expected = generator.First(x => x >= 1 && x <= 12);

			var sut = new Month(expected);

			// Act
			var result = sut.MonthNumber;

			// Assert
			result.Should().Be(expected);
		}

		[Theory, AutoFakeItEasyData]
		public void TheConstructorShouldThrowAnExceptionIfThePassedInMonthNumberIsNotValid(Generator<uint> generator)
		{
			// Arrange
			uint badMonthNumber = generator.First(x => x > 12);

			// Act
			Action action = () => new Month(badMonthNumber);

			// Assert
			action.ShouldThrow<ArgumentOutOfRangeException>();
		}

		[Fact]
		public void JanuaryShouldHaveTheCorrectMonthNumber()
		{
			// Assert
			Month.January.MonthNumber.Should().Be(1);
		}

		[Fact]
		public void FebruaryShouldHaveTheCorrectMonthNumber()
		{
			// Assert
			Month.February.MonthNumber.Should().Be(2);
		}

		[Fact]
		public void MarchShouldHaveTheCorrectMonthNumber()
		{
			// Assert
			Month.March.MonthNumber.Should().Be(3);
		}

		[Fact]
		public void AprilShouldHaveTheCorrectMonthNumber()
		{
			// Assert
			Month.April.MonthNumber.Should().Be(4);
		}

		[Fact]
		public void MayShouldHaveTheCorrectMonthNumber()
		{
			// Assert
			Month.May.MonthNumber.Should().Be(5);
		}

		[Fact]
		public void JuneShouldHaveTheCorrectMonthNumber()
		{
			// Assert
			Month.June.MonthNumber.Should().Be(6);
		}

		[Fact]
		public void JulyShouldHaveTheCorrectMonthNumber()
		{
			// Assert
			Month.July.MonthNumber.Should().Be(7);
		}

		[Fact]
		public void AugustShouldHaveTheCorrectMonthNumber()
		{
			// Assert
			Month.August.MonthNumber.Should().Be(8);
		}

		[Fact]
		public void SeptemberShouldHaveTheCorrectMonthNumber()
		{
			// Assert
			Month.September.MonthNumber.Should().Be(9);
		}

		[Fact]
		public void OctoberShouldHaveTheCorrectMonthNumber()
		{
			// Assert
			Month.October.MonthNumber.Should().Be(10);
		}

		[Fact]
		public void NovemberShouldHaveTheCorrectMonthNumber()
		{
			// Assert
			Month.November.MonthNumber.Should().Be(11);
		}

		[Fact]
		public void DecemberShouldHaveTheCorrectMonthNumber()
		{
			// Assert
			Month.December.MonthNumber.Should().Be(12);
		}
	}
}