using System;
using System.Collections.Generic;
using System.Linq;
using Common.UnitTests.TestingHelpers;
using FakeItEasy;
using FluentAssertions;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Xunit;
using Ploeh.SemanticComparison.Fluent;
using SecuritySystemDSL.SemanticModel;
using Xunit.Extensions;

// ReSharper disable CheckNamespace
namespace SecuritySystemDSL.UnitTests.SemanticModel.StateTests
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
		public void ItShouldImplementTheExpectedRoles(IFixture fixture)
		{
			// Arrange
			// Act
			var sut = fixture.Create<State>();

			// Assert
			sut.Should().BeAssignableTo<IState>();
		}

		[Theory, AutoFakeItEasyData]
		public void AllConstructorArgumentsShouldBeExposedAsWellBehavedReadOnlyProperties(IFixture fixture)
		{
			// Arrange
			var assertion = new ConstructorInitializedMemberAssertion(fixture);
			var type = typeof(State);

			// Act
			var constructors = type.GetConstructors();
			var readOnlyProperties = type.GetProperties().Where(x => x.GetSetMethod(nonPublic: true) == null && x.Name != "Transitions" && x.Name != "Actions");

			// Assert
			assertion.Verify(constructors);
			assertion.Verify(readOnlyProperties);
		}
	}

	public class WhenAddingTransitions
	{
		[Theory, AutoFakeItEasyData]
		public void ItShouldUseTheEventCodeAsAKey(IFixture fixture, Event trigger, State targetState)
		{
			// Arrange
			var expectedKey = trigger.Code;

			var sut = fixture.Create<State>();

			// Act
			sut.AddTransition(trigger, targetState);

			// Assert
			sut.Transitions.Should().HaveCount(1);
			sut.Transitions.Single().Key.Should().Be(expectedKey);
		}

		[Theory, AutoFakeItEasyData]
		public void ItShouldAddTheExpectedTransition(IFixture fixture, Event trigger, State targetState)
		{
			// Arrange
			var sut = fixture.Create<State>();

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
			likeness.ShouldEqual(sut.Transitions.Single().Value);
		}
	}

	public class WhenCheckingIfStateContainsATransition
	{
		[Theory, AutoFakeItEasyData]
		public void ItShouldReturnTrueIfTheStateContainsTheTransition(IFixture fixture, Event trigger, State targetState)
		{
			// Arrange
			var sut = fixture.Create<State>();

			sut.AddTransition(trigger, targetState);

			// Act
			var result = sut.HasTransition(trigger.Code);

			// Assert
			result.Should().BeTrue();
		}

		[Theory, AutoFakeItEasyData]
		public void ItShouldReturnFalseIfTheStateDoesNotContainTheTransition(IFixture fixture, string eventCode)
		{
			// Arrange
			var sut = fixture.Create<State>();

			// Act
			var result = sut.HasTransition(eventCode);

			// Assert
			result.Should().BeFalse();
		}
	}

	public class WhenGettingTheTargetStateForAnEventCode
	{
		[Theory, AutoFakeItEasyData]
		public void ItShouldReturnTheExpectedTargetStateIfTheEventCodeIsUsed(IFixture fixture, Event trigger, State targetState)
		{
			// Arrange
			var sut = fixture.Create<State>();

			sut.AddTransition(trigger, targetState);

			// Act
			var result = sut.FindTargetState(trigger.Code);

			// Assert
			result.Should().BeSameAs(targetState);
		}

		[Theory, AutoFakeItEasyData]
		public void ItShouldThrowAKeyNotFoundExceptionIfTheEventCodeIsNotUsed(IFixture fixture, string eventCode)
		{
			// Arrange
			var sut = fixture.Create<State>();

			// Act
			Action action = () => sut.FindTargetState(eventCode);

			// Assert
			action.ShouldThrow<KeyNotFoundException>();
		}
	}

	public class WhenAddingActions
	{
		[Theory, AutoFakeItEasyData]
		public void ItShouldAddThePassedInCommandAsAnAction(IFixture fixture, Command command)
		{
			// Arrange
			var sut = fixture.Create<State>();

			// Act
			sut.AddAction(command);

			// Assert
			sut.Actions.Should().HaveCount(1);
			sut.Actions.Single().Should().Be(command);
		}
	}

	public class WhenExecutingActions
	{
//		[Theory, AutoFakeItEasyData]
//		public void ItShouldCallTheCommandChannelsSendMethodForEachAction(IFixture fixture, List<Command> commands, [Frozen]ICommandChannel commandChannel)
//		{
//			// Arrange
//			var sut = fixture.Create<State>();
//
//			commands.ForEach(sut.AddAction);
//
//			// Act
//			sut.ExecuteActions(commandChannel);
//
//			// Assert
//			A.CallTo(() => commandChannel.Send(A<string>._)).MustHaveHappened(Repeated.Exactly.Times(commands.Count));
//		}

		[Theory, AutoFakeItEasyData]
		public void ItShouldCallTheCommandChannelsSendMethodWithTheCorrectCommandCodes(IFixture fixture, List<Command> commands, [Frozen]ICommandChannel commandChannel)
		{
			// Arrange
			var sut = fixture.Create<State>();

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