using System;
using System.Linq;
using DSLExamples.RecurringEvents.SemanticModel;
using FluentAssertions;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Idioms;
using Xunit;
using Xunit.Extensions;

// ReSharper disable CheckNamespace
namespace DSLExamples.UnitTests.RecurringEvents.SemanticModel.PeriodInYearTests
// ReSharper restore CheckNamespace
{
	public class WhenVerifyingArchitecturalConstraints
	{
		[Theory, ValidMonth]
		public void AllConstructorArgumentsShouldBeExposedAsWellBehavedReadOnlyProperties(IFixture fixture)
		{
			// Arrange
			var assertion = new ConstructorInitializedMemberAssertion(fixture);
			var type = typeof(PeriodInYear);

			// Act
			var constructors = type.GetConstructors();
			var readOnlyProperties = type.GetProperties().Where(x => x.GetSetMethod(nonPublic: true) == null);

			// Assert
			assertion.Verify(constructors);
			assertion.Verify(readOnlyProperties);
		}

		[Theory, ValidMonth]
		public void ItShouldBeAnISpecification(IFixture fixture, PeriodInYear sut)
		{
			sut.Should().BeAssignableTo<ISpecification<DateTime>>();
		}
	}

	public class WhenTestingIfSpecificationIsCorrect
	{
		// Use a year and a day that is not also a valid month number
		const int DUMMY_YEAR = 2013;
		const int DUMMY_DAY = 20;

		[Fact]
		public void ItShouldReturnTrueIfDateMonthIsEqualToStartMonth()
		{
			// Arrange
			var startMonth = new Month(1);
			var endMonth = new Month(12);

			var dateToTest = new DateTime(DUMMY_YEAR, startMonth.Number, DUMMY_DAY);

			var sut = new PeriodInYear(startMonth, endMonth);

			// Act
			var result = sut.IsSatisfiedBy(dateToTest);

			// Assert
			result.Should().BeTrue();
		}

		[Fact]
		public void ItShouldReturnTrueIfDateMonthIsEqualToEndMonth()
		{
			// Arrange
			var startMonth = new Month(1);
			var endMonth = new Month(12);

			var dateToTest = new DateTime(DUMMY_YEAR, endMonth.Number, DUMMY_DAY);

			var sut = new PeriodInYear(startMonth, endMonth);

			// Act
			var result = sut.IsSatisfiedBy(dateToTest);

			// Assert
			result.Should().BeTrue();
		}

		[Fact]
		public void ItShouldReturnTrueIfDateMonthIsBetweenStartAndEnd()
		{
			// Arrange
			var startMonth = new Month(1);
			var endMonth = new Month(12);
			var monthBetweenStartAndEnd = new Month(6);

			var dateToTest = new DateTime(DUMMY_YEAR, monthBetweenStartAndEnd.Number, DUMMY_DAY);

			var sut = new PeriodInYear(startMonth, endMonth);

			// Act
			var result = sut.IsSatisfiedBy(dateToTest);

			// Assert
			result.Should().BeTrue();
		}

		[Fact]
		public void ItShouldReturnFalseIfDateMonthIsBeforeStartMonth()
		{
			// Arrange
			var startMonth = new Month(2);
			var endMonth = new Month(12);
			var monthBeforeStart = new Month(1);

			var dateToTest = new DateTime(DUMMY_YEAR, monthBeforeStart.Number, DUMMY_DAY);

			var sut = new PeriodInYear(startMonth, endMonth);

			// Act
			var result = sut.IsSatisfiedBy(dateToTest);

			// Assert
			result.Should().BeFalse();
		}

		[Fact]
		public void ItShouldReturnFalseIfDateMonthIsAfterEndMonth()
		{
			// Arrange
			var startMonth = new Month(1);
			var endMonth = new Month(11);
			var monthAfterEnd = new Month(12);

			var dateToTest = new DateTime(DUMMY_YEAR, monthAfterEnd.Number, DUMMY_DAY);

			var sut = new PeriodInYear(startMonth, endMonth);

			// Act
			var result = sut.IsSatisfiedBy(dateToTest);

			// Assert
			result.Should().BeFalse();
		}
	}
}