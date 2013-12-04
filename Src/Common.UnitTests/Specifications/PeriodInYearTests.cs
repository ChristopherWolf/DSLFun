using System;
using System.Collections.Generic;
using System.Linq;
using Common.Specifications;
using Common.UnitTests.TestingHelpers;
using FluentAssertions;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Idioms;
using Xunit.Extensions;

// ReSharper disable CheckNamespace
namespace Common.UnitTests.Specifications.Dates.PeriodInYearTests
// ReSharper restore CheckNamespace
{
	public class WhenVerifyingArchitecturalConstraints
	{
		[Theory, AutoFakeItEasyData]
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

		[Theory, AutoFakeItEasyData]
		public void ItShouldBe(IFixture fixture, PeriodInYear sut)
		{
			sut.Should().BeAssignableTo<ISpecification<DateTime>>();
		}
	}

	public class WhenTestingIfSpecificationIsCorrect
	{
		public static IEnumerable<object[]> TestData 
		{
			get
			{
				// Date's Month equal to start month
				yield return CreateItems(startMonth: 1, endMonth: 12, dateMonth: 1);

				// Date's Month equal to end month
				yield return CreateItems(startMonth: 1, endMonth: 12, dateMonth: 12);

				// Date's Month between start and end
				yield return CreateItems(startMonth: 1, endMonth: 12, dateMonth: 6);

				// Date's Month before start month
				yield return CreateItems(startMonth: 5, endMonth: 12, dateMonth: 1);

				// Date's Month after end month
				yield return CreateItems(startMonth: 1, endMonth: 5, dateMonth: 12);
			}
		}

		static object[] CreateItems(int startMonth, int endMonth, int dateMonth)
		{
			var rand = new Random();

			int year = rand.Next(1, 9999);
			int day = rand.Next(1, 28);

			var date = new DateTime(year, dateMonth, day);

			var expectedValue = date.Month >= startMonth && date.Month <= endMonth;

			return new object[] { startMonth, endMonth, date, expectedValue };
		}

		[Theory, PropertyData("TestData")]
		public void ItShouldReturnTheCorrectValue(int startMonth, int endMonth, DateTime testDate, bool expectedResult)
		{
			// Arrange
			var sut = new PeriodInYear(startMonth, endMonth);

			// Act
			var result = sut.IsSatisfiedBy(testDate);

			// Assert
			result.Should().Be(expectedResult);
		}
	}
}