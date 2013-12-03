using System;
using Common.Specifications;
using Common.UnitTests.TestingHelpers;
using DSLExamples.RecurringEvents.SemanticModel;
using FluentAssertions;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Idioms;
using Xunit.Extensions;

// ReSharper disable CheckNamespace
namespace DSLExamples.UnitTests.RecurringEvents.SemanticModel.DayOfWeekInAMonthSpecificationTests
// ReSharper restore CheckNamespace
{
	public class WhenVerifyingArchitecturalConstraints
	{
		[Theory, AutoFakeItEasyData]
		public void AllMethodsShouldHaveProperGuardClauses(IFixture fixture)
		{
			// Arrange
			var assertion = new GuardClauseAssertion(fixture);

			// Act
			Type sutType = typeof (DayOfWeekInAMonthSpecification);

			// Assert
			assertion.Verify(sutType);
		}

		[Theory, AutoFakeItEasyData]
		public void ItShouldBeAnISpecification(IFixture fixture, DayOfWeekInAMonthSpecification sut)
		{
			sut.Should().BeAssignableTo<ISpecification<DateTime>>();
		}

		[Theory]
		[InlineData(-1)]
		[InlineData(0)]
		public void ItShouldThrowAnExceptionForInvalidIndexes(int index)
		{
			// Arrange

			// Act
			Action action = () => new DayOfWeekInAMonthSpecification(index, DayOfWeek.Monday);

			// Assert
			action.ShouldThrow<ArgumentOutOfRangeException>();
		}
	}

	public class WhenTestingIfSpecificationIsCorrect
	{
		[Theory]
		[InlineData(2014, 4, 15, 3, DayOfWeek.Tuesday, true)]
		[InlineData(2013, 12, 1, 1, DayOfWeek.Sunday, true)]
		[InlineData(2013, 12, 29, 5, DayOfWeek.Sunday, true)]
		[InlineData(2013, 12, 29, 4, DayOfWeek.Sunday, false)]
		[InlineData(2014, 4, 15, 2, DayOfWeek.Tuesday, false)]
		[InlineData(2013, 12, 1, 3, DayOfWeek.Tuesday, false)]
		public void ItShouldReturnTheExpectedValueWhenCheckingThePassedInDate(int year, int month, int day, int index,
		                                                                      DayOfWeek dayOfWeek, bool expected)
		{
			// Arrange
			var dateTime = new DateTime(year, month, day);
			var sut = new DayOfWeekInAMonthSpecification(index, dayOfWeek);

			// Act
			bool result = sut.IsSatisfiedBy(dateTime);

			// Assert
			result.Should().Be(expected);
		}

		[Theory]
		[InlineData(2014, 4, 15, 3, DayOfWeek.Tuesday, true)]
		[InlineData(2013, 12, 1, 1, DayOfWeek.Sunday, true)]
		[InlineData(2013, 12, 29, 5, DayOfWeek.Sunday, true)]
		[InlineData(2013, 12, 29, 4, DayOfWeek.Sunday, false)]
		[InlineData(2014, 4, 15, 2, DayOfWeek.Tuesday, false)]
		[InlineData(2013, 12, 1, 3, DayOfWeek.Tuesday, false)]
		public void ItShouldReturnTheExpectedValueWhenCheckingThePassedInDateIgnoringTheTime(int year, int month, int day, int index,
																			  DayOfWeek dayOfWeek, bool expected)
		{
			// Arrange
			var dateTime = new DateTime(year, month, day, 5, 5, 55);
			var sut = new DayOfWeekInAMonthSpecification(index, dayOfWeek); 

			// Act
			bool result = sut.IsSatisfiedBy(dateTime);

			// Assert
			result.Should().Be(expected);
		}
	}
}