using System;
using System.Collections.Generic;
using System.Linq;
using Common.UnitTests.TestingHelpers;
using FakeItEasy;
using FluentAssertions;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoFakeItEasy;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Xunit;
using Ploeh.SemanticComparison.Fluent;
using SecuritySystemDSL.SemanticModel;
using Xunit.Extensions;

// ReSharper disable CheckNamespace
namespace SecuritySystemDSL.UnitTests.SemanticModel.StateMachineTests
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
			Type sutType = typeof(StateMachine);

			// Assert
			assertion.Verify(sutType);
		}

		[Theory, AutoFakeItEasyData]
		public void AllConstructorArgumentsShouldBeExposedAsWellBehavedReadOnlyProperties(IFixture fixture)
		{
			// Arrange
			var assertion = new ConstructorInitializedMemberAssertion(fixture);
			var type = typeof(StateMachine);

			// Act
			var constructors = type.GetConstructors();
			var readOnlyProperties = type.GetProperties().Where(x => x.GetSetMethod(nonPublic: true) == null && x.Name != "ResetEvents");

			// Assert
			assertion.Verify(constructors);
			assertion.Verify(readOnlyProperties);
		}

		[Theory, AutoFakeItEasyData]
		public void ItShouldImplementTheExpectedRoles(IFixture fixture)
		{
			// Arrange
			// Act
			var sut = fixture.Create<StateMachine>();

			// Assert
			sut.Should().BeAssignableTo<IStateMachine>();
		}
	}

	public class WhenAddingResetEvents
	{
		[Theory, AutoFakeItEasyData]
		public void ItShouldAddThePassedInEventAsAResetEvent(IFixture fixture, Event resetEvent)
		{
			// Arrange
			var sut = fixture.Create<StateMachine>();

			// Act
			sut.AddResetEvent(resetEvent);

			// Assert
			sut.ResetEvents.Should().HaveCount(1);
			sut.ResetEvents.Single().Should().Be(resetEvent);
		}
	}

	public class WhenCheckingIfAnEventIsAResetEvent
	{
		[Theory, AutoFakeItEasyData]
		public void ItShouldReturnTrueIfTheEventIsAResetEvent(IFixture fixture, Event @event)
		{
			// Arrange
			var sut = fixture.Create<StateMachine>();

			sut.AddResetEvent(@event);

			// Act
			var result = sut.IsResetEvent(@event.Code);

			// Assert
			result.Should().BeTrue();
		}

		[Theory, AutoFakeItEasyData]
		public void ItShouldReturnFalseIfTheEventIsNotAResetEvent(IFixture fixture, string eventCode)
		{
			// Arrange
			var sut = fixture.Create<StateMachine>();

			// Act
			var result = sut.IsResetEvent(eventCode);

			// Assert
			result.Should().BeFalse();
		}
	}
}