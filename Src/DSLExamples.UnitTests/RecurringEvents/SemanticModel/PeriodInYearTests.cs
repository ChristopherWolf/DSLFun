using System;
using System.Linq;
using Common.Specifications;
using DSLExamples.RecurringEvents.SemanticModel;
using FluentAssertions;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Idioms;
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
		[Theory, ValidMonth]
		public void ItShouldReturnCorrectValueIfDateMonthIsEqualToStartMonth(MonthTuple monthTuple, Generator<DateTime> dateTimeGenerator)
		{
			// Arrange
			var startMonth = monthTuple.StartMonth;
			var endMonth = monthTuple.EndMonth;

			var sut = new PeriodInYear(startMonth, endMonth);

			var dateToTest = dateTimeGenerator.First(x => x.Month == startMonth.Number);

			var expectedResult = dateToTest.Month >= startMonth.Number && dateToTest.Month <= endMonth.Number;

			// Act
			var result = sut.IsSatisfiedBy(dateToTest);

			// Assert
			result.Should().BeTrue();
		}

		[Theory, ValidMonth]
		public void ItShouldReturnCorrectValueIfDateMonthIsEqualToEndMonth(MonthTuple monthTuple, Generator<DateTime> dateTimeGenerator)
		{
			// Arrange
			var startMonth = monthTuple.StartMonth;
			var endMonth = monthTuple.EndMonth;

			var sut = new PeriodInYear(startMonth, endMonth);

			var dateToTest = dateTimeGenerator.First(x => x.Month == endMonth.Number);

			var expectedResult = dateToTest.Month >= startMonth.Number && dateToTest.Month <= endMonth.Number;

			// Act
			var result = sut.IsSatisfiedBy(dateToTest);

			// Assert
			result.Should().Be(expectedResult);
		}

		[Theory, ValidMonth]
		public void ItShouldReturnCorrectValueIfDateMonthIsBetweenStartAndEnd(MonthTuple monthTuple, Generator<DateTime> dateTimeGenerator)
		{
			// Arrange
			var startMonth = monthTuple.StartMonth;
			var endMonth = monthTuple.EndMonth;

			var sut = new PeriodInYear(startMonth, endMonth);

			var dateToTest = dateTimeGenerator.First(x => x.Month > startMonth.Number && x.Month < endMonth.Number);

			var expectedResult = dateToTest.Month >= startMonth.Number && dateToTest.Month <= endMonth.Number;

			// Act
			var result = sut.IsSatisfiedBy(dateToTest);

			// Assert
			result.Should().Be(expectedResult);
		}

		[Theory, ValidMonth]
		public void ItShouldReturnCorrectValueIfDateMonthIsBeforeStartMonth(MonthGenerator monthGenerator, Generator<DateTime> dateTimeGenerator)
		{
			// Arrange
			var startMonth = monthGenerator.First(x => x.Number > 1 && x.Number < 12);
			var endMonth = new Month(startMonth.Number + 1);

			var sut = new PeriodInYear(startMonth, endMonth);

			var dateToTest = dateTimeGenerator.First(x => x.Month < startMonth.Number);

			var expectedResult = dateToTest.Month >= startMonth.Number && dateToTest.Month <= endMonth.Number;

			// Act
			var result = sut.IsSatisfiedBy(dateToTest);

			// Assert
			result.Should().Be(expectedResult);
		}

		[Theory, ValidMonth]
		public void ItShouldReturnCorrectValueIfDateMonthIsAfterEndMonth(MonthGenerator monthGenerator, Generator<DateTime> dateTimeGenerator)
		{
			// Arrange
			var endMonth = monthGenerator.First(x => x.Number > 1 && x.Number <= 12);
			var startMonth = new Month(endMonth.Number - 1);

			var sut = new PeriodInYear(startMonth, endMonth);

			var dateToTest = dateTimeGenerator.First(x => x.Month > endMonth.Number);

			var expectedResult = dateToTest.Month >= startMonth.Number && dateToTest.Month <= endMonth.Number;

			// Act
			var result = sut.IsSatisfiedBy(dateToTest);

			// Assert
			result.Should().Be(expectedResult);
		}
	}
}