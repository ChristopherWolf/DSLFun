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

			fixture.Register(() =>
				{
					var monthNumber = generator.First(x => x >= 1 && x <= 12);

					return new Month(monthNumber);
				});
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
		public void ItShouldReturnTheCorrectCompositeSpecification(IFixture fixture)
		{
			// Arrange
			var lhsSpec = fixture.Create<ISpecification<DateTime>>();
			A.CallTo(() => lhsSpec.ToString()).Returns("LHS Spec");

			var rhsSpec = fixture.Create<ISpecification<DateTime>>();
			A.CallTo(() => rhsSpec.ToString()).Returns("RHS Spec");

			var additional = new Schedule(rhsSpec);
			var sut = new Schedule(lhsSpec);

			var likness = lhsSpec.AsSource()
								.OfLikeness<OrSpecification<DateTime>>()
								.With(x => x.LHS).EqualsWhen((single, composite) => composite.LHS == lhsSpec)
								.With(x => x.RHS).EqualsWhen((single, composite) => composite.RHS == rhsSpec);

			// Act
			var result = sut.And(additional);

			// Assert
			result.Should().NotBeNull();
			result.Content.Should().NotBeNull();
			result.Content.Should().BeOfType<OrSpecification<DateTime>>();

			likness.ShouldEqual(result.Content.As<OrSpecification<DateTime>>());
		}

		[Theory, AutoFakeItEasyData]
		public void ItShouldReturnTheSutToContinueTheFluentChain(IFixture fixture, Schedule additional)
		{
			// Arrange
			var sut = fixture.Create<Schedule>();

			// Act
			var result = sut.And(additional);

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
			result.PeriodStart.Should().BeSameAs(month);
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

	public class WhenTestingTheUntilMethod
	{
		[Theory, ValidMonth]
		public void ItShouldReturnAnAndCompositeSpecificationWithTheCorrectLHS(IFixture fixture, Month startMonth, Month endMonth)
		{
			// Arrange
			var lhsSpec = fixture.Create<ISpecification<DateTime>>();
			A.CallTo(() => lhsSpec.ToString()).Returns("LHS Spec");

			var sut = new Schedule(lhsSpec);

			sut.From(startMonth);

			// Act
			var result = sut.Until(endMonth);

			// Assert
			result.Should().NotBeNull();
			result.Content.Should().NotBeNull();
			result.Content.Should().BeOfType<AndSpecification<DateTime>>();

			var compositeSpec = result.Content.As<AndSpecification<DateTime>>();

			compositeSpec.LHS.Should().BeSameAs(lhsSpec);
		}

		[Theory, ValidMonth]
		public void ItShouldReturnAnAndCompositeSpecificationWithTheCorrectRHS(IFixture fixture, Month startMonth, Month endMonth)
		{
			// Arrange
			var rhsSpec = fixture.Create<ISpecification<DateTime>>();
			A.CallTo(() => rhsSpec.ToString()).Returns("RHS Spec");

			var expectedSpec = new PeriodInYear(startMonth.MonthNumber, endMonth.MonthNumber);

			var sut = fixture.Create<Schedule>();

			sut.From(startMonth);

			var likness = expectedSpec.AsSource()
			                      .OfLikeness<PeriodInYear>();

			// Act
			var result = sut.Until(endMonth);

			// Assert
			result.Should().NotBeNull();
			result.Content.Should().NotBeNull();
			result.Content.Should().BeOfType<AndSpecification<DateTime>>();

			var compositeSpec = result.Content.As<AndSpecification<DateTime>>();

			compositeSpec.RHS.Should().BeOfType<PeriodInYear>();

			var periodInYearSpec = compositeSpec.RHS.As<PeriodInYear>();

			likness.ShouldEqual(periodInYearSpec);
		}

		[Theory, ValidMonth]
		public void ItShouldReturnTheSutToContinueTheFluentChain(IFixture fixture, Month startMonth, Month endMonth)
		{
			// Arrange
			var sut = fixture.Create<Schedule>();

			sut.From(startMonth);

			// Act
			var result = sut.Until(endMonth);

			// Assert
			result.Should().BeSameAs(sut);
		} 
	}
}