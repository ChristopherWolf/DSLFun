using System;
using System.Collections.Generic;
using System.Linq;
using Common.UnitTests.TestingHelpers;
using DSLExamples.RecurringEvents.SemanticModel;
using FluentAssertions;
using Ploeh.AutoFixture;
using Xunit.Extensions;

// ReSharper disable CheckNamespace
namespace DSLExamples.UnitTests.RecurringEvents.SemanticModel.DaysInMonthEnumeratorTests
// ReSharper restore CheckNamespace
{
	public class WhenVerifyingArchitecturalConstraints
	{
		[Theory, AutoFakeItEasyData]
		public void ItShouldBeAnIEnumerable(IFixture fixture, DateTime date)
		{
			// Arrange
			var sut = new DaysInMonthEnumerator(date.Month, date.Year);

			// Act and Assert
			sut.Should().BeAssignableTo<IEnumerable<DateTime>>();
		}
	}

	public class WhenEnumeratingTheSut
	{
		[Theory, AutoFakeItEasyData]
		public void ItShouldReturnTheExpectedDays(DateTime date)
		{
			// Arrange
			var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
			var firstDayOfNextMonth = firstDayOfMonth.AddMonths(1);

			var expected = GetAllIndividualDaysBetween(firstDayOfMonth, firstDayOfNextMonth).ToArray();

			var sut = new DaysInMonthEnumerator(date.Month, date.Year);

			// Act
			var result = sut.ToArray();

			// Assert
			result.Should().Equal(expected);
		}

		IEnumerable<DateTime> GetAllIndividualDaysBetween(DateTime start, DateTime end)
		{
			// Remove time info from start date (we only care about day). 
			var current = new DateTime(start.Year, start.Month, start.Day);

			while (current < end)
			{
				yield return current;
				current = current.AddDays(1);
			}
		}
	}
}