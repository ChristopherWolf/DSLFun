using System;
using System.Linq;
using Common.Specifications;
using Common.UnitTests.TestingHelpers;
using DSLExamples.RecurringEvents.InternalDSL;
using DSLExamples.RecurringEvents.SemanticModel;
using FakeItEasy;
using FluentAssertions;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoFakeItEasy;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Xunit;
using Ploeh.SemanticComparison.Fluent;
using Xunit.Extensions;

// ReSharper disable CheckNamespace
namespace DSLExamples.UnitTests.RecurringEvents.InternalDSL.ScheduleTests
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
			Type sutType = typeof (Schedule);

			// Assert
			assertion.Verify(sutType);
		}

		[Theory, AutoFakeItEasyData]
		public void AllConstructorArgumentsShouldBeExposedAsWellBehavedReadOnlyProperties(IFixture fixture)
		{
			// Arrange
			var assertion = new ConstructorInitializedMemberAssertion(fixture);
			var type = typeof(Schedule);

			// Act
			var constructors = type.GetConstructors();
			var readOnlyProperties = type.GetProperties().Where(x => x.GetSetMethod(nonPublic: true) == null);

			// Assert
			assertion.Verify(constructors);
			assertion.Verify(readOnlyProperties);
		}
	}

	public class WhenTestingTheFirstMethod
	{
		[Theory]
		[InlineData(DayOfWeek.Monday)]
		[InlineData(DayOfWeek.Tuesday)]
		[InlineData(DayOfWeek.Wednesday)]
		[InlineData(DayOfWeek.Thursday)]
		[InlineData(DayOfWeek.Friday)]
		[InlineData(DayOfWeek.Saturday)]
		[InlineData(DayOfWeek.Sunday)]
		public void ItShouldReturnTheExpectedInitializedSchedule(DayOfWeek dayOfWeek)
		{
			// Arrange
			const int expectedIndex = 1;

			var likeness = dayOfWeek.AsSource()
			                        .OfLikeness<DayOfWeekInAMonthSpecification>()
			                        .With(x => x.Index).EqualsWhen((dow, spec) => spec.Index == expectedIndex)
									.With(x => x.DayOfWeek).EqualsWhen((dow, spec) => spec.DayOfWeek == dow);

			// Act
			var result = Schedule.First(dayOfWeek);

			// Assert
			result.Should().NotBeNull();
			result.Content.Should().BeOfType<DayOfWeekInAMonthSpecification>();

			likeness.ShouldEqual(result.Content.As<DayOfWeekInAMonthSpecification>());
		} 
	}

	public class WhenTestingTheAndMethod
	{
		[Theory, AutoFakeItEasyData]
		public void ItShouldOrTheSpecifications(IFixture fixture, ISpecification<DateTime> firstContent, ISpecification<DateTime> secondContent)
		{
			// Arrange
			var newSchedule = new Schedule(secondContent);
			var sut = new Schedule(firstContent);

			// Act
			var result = sut.And(newSchedule);

			// Assert
			result.Should().NotBeNull();
			result.Content.Should().BeOfType<OrSpecification<DateTime>>();

			var spec = result.Content.As<OrSpecification<DateTime>>();

			spec.InnerSpecifications.Should().ContainInOrder(firstContent, secondContent);
		}

		[Theory, AutoFakeItEasyData]
		public void ItShouldReturnTheSutToContinueTheFluentChain(IFixture fixture, Schedule newSchedule)
		{
			// Arrange
			var sut = fixture.Create<Schedule>();

			// Act
			var result = sut.And(newSchedule);

			// Assert
			result.Should().BeSameAs(sut);
		} 
	}
}