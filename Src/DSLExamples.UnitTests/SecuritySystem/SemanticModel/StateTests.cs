﻿using System;
using System.Collections.Generic;
using System.Linq;
using Common.UnitTests.TestingHelpers;
using DSLExamples.SecuritySystem.SemanticModel;
using FakeItEasy;
using FluentAssertions;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Xunit;
using Ploeh.SemanticComparison.Fluent;
using Xunit.Extensions;

// ReSharper disable CheckNamespace
namespace DSLExamples.SecuritySystem.SecuritySystem.UnitTests.SemanticModel.StateTests
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
			Type sutType = typeof(State);

			// Assert
			assertion.Verify(sutType);
		}

		[Theory, AutoFakeItEasyData]
		public void AllConstructorArgumentsShouldBeExposedAsWellBehavedReadOnlyProperties(IFixture fixture)
		{
			// Arrange
			var assertion = new ConstructorInitializedMemberAssertion(fixture);
			var type = typeof(State);

			// Act
			var constructors = type.GetConstructors();
			var readOnlyProperties = type.GetProperties().Where(x => x.GetSetMethod(nonPublic: true) == null && x.Name != "Transitions");

			// Assert
			assertion.Verify(constructors);
			assertion.Verify(readOnlyProperties);
		}
	}

	public class WhenAddingTransitions
	{
		[Theory, AutoFakeItEasyData]
		public void ItShouldUseTheTriggersEventCode(Event trigger, IState targetState, State sut)
		{
			// Arrange
			var expected = trigger.Code;

			// Act
			sut.AddTransition(trigger, targetState);

			// Assert
			sut.HasTransition(expected).Should().BeTrue();
		}

		[Theory, AutoFakeItEasyData]
		public void ItShouldAddTheExpectedTransition(Event trigger, IState targetState, State sut)
		{
			// Arrange
			var likeness = sut.AsSource()
			                  .OfLikeness<Transition>()
							  .OmitAutoComparison()
			                  .With(x => x.Source).EqualsWhen((inputState, transition) => transition.Source == inputState)
							  .With(x => x.Target).EqualsWhen((inputState, transition) => transition.Target == targetState)
							  .With(x => x.Trigger).EqualsWhen((inputState, transition) => transition.Trigger == trigger);

			// Act
			sut.AddTransition(trigger, targetState);

			// Assert
			sut.Transitions.Should().HaveCount(1);
			likeness.ShouldEqual(sut.Transitions.Single());
		}
	}

	public class WhenCheckingIfStateContainsATransition
	{
		[Theory, AutoFakeItEasyData]
		public void ItShouldReturnTrueIfTheStateContainsTheTransition(Event trigger, IState targetState, State sut)
		{
			// Arrange
			sut.AddTransition(trigger, targetState);

			// Act
			var result = sut.HasTransition(trigger.Code);

			// Assert
			result.Should().BeTrue();
		}

		[Theory, AutoFakeItEasyData]
		public void ItShouldReturnFalseIfTheStateDoesNotContainTheTransition(string eventCode, State sut)
		{
			// Arrange

			// Act
			var result = sut.HasTransition(eventCode);

			// Assert
			result.Should().BeFalse();
		}
	}

	public class WhenGettingTheTargetStateForAnEventCode
	{
		[Theory, AutoFakeItEasyData]
		public void ItShouldReturnTheExpectedTargetStateIfTheEventCodeIsUsed(Event trigger, IState targetState, State sut)
		{
			// Arrange
			sut.AddTransition(trigger, targetState);

			// Act
			var result = sut.FindTargetState(trigger.Code);

			// Assert
			result.Should().BeSameAs(targetState);
		}

		[Theory, AutoFakeItEasyData]
		public void ItShouldThrowAKeyNotFoundExceptionIfTheEventCodeIsNotUsed(string eventCode, State sut)
		{
			// Arrange

			// Act
			Action action = () => sut.FindTargetState(eventCode);

			// Assert
			action.ShouldThrow<KeyNotFoundException>();
		}
	}

	public class WhenAddingActions
	{
		[Theory, AutoFakeItEasyData]
		public void ItShouldAddThePassedInCommandAsAnAction(ICommandChannel commandChannel, Command command, State sut)
		{
			// Arrange

			// Act
			sut.AddAction(command);

			sut.ExecuteActions(commandChannel);

			// Assert
			A.CallTo(() => commandChannel.Send(command.Code)).MustHaveHappened(Repeated.Exactly.Once);
		}
	}

	public class WhenExecutingActions
	{
		[Theory, AutoFakeItEasyData]
		public void ItShouldCallTheCommandChannelsSendMethodWithTheCorrectCommandCodes(List<Command> commands, [Frozen]ICommandChannel commandChannel, State sut)
		{
			// Arrange
			commands.ForEach(sut.AddAction);

			var expected = commands.Select(x => x.Code);

			var actual = new List<string>();

			A.CallTo(() => commandChannel.Send(A<string>._)).Invokes(x =>
				{
					var a = x.GetArgument<string>(0);

					actual.Add(a);
				});

			// Act
			sut.ExecuteActions(commandChannel);

			// Assert
			actual.Should().Equal(expected);
		}
	}
}