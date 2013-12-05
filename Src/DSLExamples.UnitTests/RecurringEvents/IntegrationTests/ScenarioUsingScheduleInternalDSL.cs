using System;
using System.Collections.Generic;
using DSLExamples.RecurringEvents.InternalDSL;
using DSLExamples.RecurringEvents.SemanticModel;
using FluentAssertions;
using Xunit.Extensions;

// ReSharper disable CheckNamespace
namespace DSLExamples.UnitTests.RecurringEvents.IntegrationTests.ScenarioUsingScheduleInternalDSL
// ReSharper restore CheckNamespace
{
	public class ScenarioUsingScheduleInternalDSL
	{
		const int YEAR = 2013;

		public static IEnumerable<object[]> TestDateTimes 
		{ 
			  get 
			  {
				  yield return new object[] { new DateTime(YEAR, 4, 1), true }; 
				  yield return new object[] { new DateTime(YEAR, 4, 15), true }; 
				  yield return new object[] { new DateTime(YEAR, 4, 12), false };
				  yield return new object[] { new DateTime(YEAR, 4, 8), false };
				  yield return new object[] { new DateTime(YEAR, 4, 22), false };

				  yield return new object[] { new DateTime(YEAR, 10, 7), true };
				  yield return new object[] { new DateTime(YEAR, 10, 21), true };
				  yield return new object[] { new DateTime(YEAR, 10, 14), false }; 

				  yield return new object[] { new DateTime(YEAR, 12, 2), false }; 
				  yield return new object[] { new DateTime(YEAR, 12, 16), false }; 
			  } 
		}

		[Theory]
		[PropertyData("TestDateTimes")]
		public void TestComplexSchedule(DateTime testDate, bool expected)
		{
			// Arrange
			var sut = Schedule.First(DayOfWeek.Monday)
			                  .And(Schedule.Third(DayOfWeek.Monday))
			                  .From(Month.April)
			                  .Until(Month.October)
			                  .Content;

			// Act
			var result = sut.IsSatisfiedBy(testDate);

			// Assert
			result.Should().Be(expected);
		}
	}
}