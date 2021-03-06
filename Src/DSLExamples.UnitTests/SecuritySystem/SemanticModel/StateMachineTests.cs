﻿using System;
using System.Linq;
using Common.UnitTests.TestingHelpers;
using FluentAssertions;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Idioms;
using DSLExamples.SecuritySystem.SemanticModel;
using Xunit.Extensions;

// ReSharper disable CheckNamespace
namespace DSLExamples.SecuritySystem.UnitTests.SemanticModel.StateMachineTests
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
			var readOnlyProperties = type.GetProperties().Where(x => x.GetSetMethod(nonPublic: true) == null);

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
		public void ItShouldAddThePassedInEventAsAResetEvent(Event resetEvent, StateMachine sut)
		{
			// Arrange

			// Act
			sut.AddResetEvent(resetEvent);

			// Assert
			sut.IsResetEvent(resetEvent.Code).Should().BeTrue();
		}
	}

	public class WhenCheckingIfAnEventIsAResetEvent
	{
		[Theory, AutoFakeItEasyData]
		public void ItShouldReturnTrueIfTheEventIsAResetEvent(Event @event, StateMachine sut)
		{
			// Arrange
			sut.AddResetEvent(@event);

			// Act
			var result = sut.IsResetEvent(@event.Code);

			// Assert
			result.Should().BeTrue();
		}

		[Theory, AutoFakeItEasyData]
		public void ItShouldReturnFalseIfTheEventIsNotAResetEvent(string eventCode, StateMachine sut)
		{
			// Arrange

			// Act
			var result = sut.IsResetEvent(eventCode);

			// Assert
			result.Should().BeFalse();
		}
	}
}