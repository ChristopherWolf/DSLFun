using System;
using System.Linq;
using Common.Specifications;
using Common.UnitTests.TestingHelpers;
using DSLExamples.RecurringEvents.InternalDSL;
using DSLExamples.RecurringEvents.SemanticModel;
using FakeItEasy;
using FluentAssertions;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Xunit;
using Ploeh.SemanticComparison.Fluent;
using Xunit.Extensions;

// ReSharper disable CheckNamespace
namespace DSLExamples.UnitTests.RecurringEvents.InternalDSL.ScheduleTests
// ReSharper restore CheckNamespace
{
	#region Customizations

	internal class ValidMonthCustomization : ICustomization
	{
		public void Customize(IFixture fixture)
		{
			var generator = fixture.Create<Generator<int>>();

			var monthNumber = generator.First(x => x > 1 && x < 12);

			fixture.Register(() => new Month(monthNumber));
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

	public class WhenVerifyingArchitecturalConstraints
	{
		[Theory, ValidMonth]
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
		public void ItShouldInitializeTheContentPropertyFromTheConstructor([Frozen] ISpecification<DateTime> content)
		{
			// Arrange
			var sut = new Schedule(content);

			// Act
			var result = sut.Content;

			// Assert
			result.Should().BeSameAs(content);
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
			A.CallTo(() => firstContent.ToString()).Returns("first");
			A.CallTo(() => secondContent.ToString()).Returns("second");

			var newSchedule = new Schedule(secondContent);
			var sut = new Schedule(firstContent);

			var expected = new[] { firstContent, secondContent };

			// Act
			var result = sut.And(newSchedule);

			// Assert
			result.Should().NotBeNull();
			result.Content.Should().BeOfType<OrSpecification<DateTime>>();

			var spec = result.Content.As<OrSpecification<DateTime>>();

			spec.InnerSpecifications.Should().Equal(expected);
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

	public class WhenTestingTheFromMethod
	{
		[Theory, ValidMonth]
		public void ItShouldSetTheStartingMonthProperty(IFixture fixture, Month month)
		{
			// Arrange
			var sut = fixture.Create<Schedule>();

			// Act
			var result = sut.From(month);

			// Assert
			result.Should().NotBeNull();
			result.StartingMonth.Should().BeSameAs(month);
		}

		[Theory, ValidMonth]
		public void ItShouldReturnTheSutToContinueTheFluentChain(IFixture fixture, Month month)
		{
			// Arrange
			var sut = fixture.Create<Schedule>();

			// Act
			var result = sut.From(month);

			// Assert
			result.Should().BeSameAs(sut);
		}
	}
}