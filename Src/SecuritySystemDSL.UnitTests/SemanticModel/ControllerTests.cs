using System;
using Common.UnitTests.TestingHelpers;
using FakeItEasy;
using FluentAssertions;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Xunit;
using SecuritySystemDSL.SemanticModel;
using Xunit.Extensions;

// ReSharper disable CheckNamespace
namespace SecuritySystemDSL.UnitTests.SemanticModel.ControllerTests
// ReSharper restore CheckNamespace
{
	#region Customizations

	internal class StartingStateHasTransitionCustomization : ICustomization
	{
		public void Customize(IFixture fixture)
		{
			var eventCode = fixture.Freeze<string>();

			var startState = fixture.Create<IState>();
			A.CallTo(() => startState.ToString()).Returns("Starting state");

			A.CallTo(() => startState.HasTransition(eventCode)).Returns(true);

			var newState = fixture.Freeze<IState>();
			A.CallTo(() => newState.ToString()).Returns("New state");

			A.CallTo(() => startState.FindTargetState(eventCode)).Returns(newState);

			var stateMachine = fixture.Freeze<IStateMachine>();
			A.CallTo(() => stateMachine.StartingState).Returns(startState);
		}
	}

	public class StartingStateHasTransitionAttribute : AutoFakeItEasyDataAttribute
	{
		public StartingStateHasTransitionAttribute()
			: base(new StartingStateHasTransitionCustomization())
		{
		}
	}

	#endregion

	public class WhenVerifyingArchitecturalConstraints
	{
		[Theory, AutoFakeItEasyData]
		public void AllMethodsShouldHaveProperGuardClauses(IFixture fixture)
		{
			// Arrange
			var assertion = new GuardClauseAssertion(fixture);

			// Act
			Type sutType = typeof(Controller);

			// Assert
			assertion.Verify(sutType);
		}

		[Theory, AutoFakeItEasyData]
		public void CurrentStateShouldBeInitializedToStateMachinesStartingState(IFixture fixture, [Frozen] IStateMachine stateMachine)
		{
			// Arrange
			var expected = stateMachine.StartingState;

			var sut = fixture.Create<Controller>();

			// Act
			var result = sut.CurrentState;

			// Assert
			result.Should().BeSameAs(expected);
		}
	}

	public class WhenHandlingAnEventCode
	{
		public class AndTheCurrentStateHasATransitonForThatEventCode
		{
			[Theory, StartingStateHasTransition]
			public void ItShouldTransitionToTheNewState(IFixture fixture, IStateMachine stateMachine, IState newState, string eventCode)
			{
				// Arrange
				var sut = fixture.Create<Controller>();

				// Act
				sut.HandleEventCode(eventCode);

				// Assert
				sut.CurrentState.Should().BeSameAs(newState);
			}

			[Theory, StartingStateHasTransition]
			public void ItShouldExecuteTheCommandsOnTheNewState(IFixture fixture, IState newState, [Frozen]ICommandChannel commandChannel, string eventCode)
			{
				// Arrange
				var sut = fixture.Create<Controller>();

				// Act
				sut.HandleEventCode(eventCode);

				// Assert
				A.CallTo(() => newState.ExecuteActions(commandChannel)).MustHaveHappened(Repeated.Exactly.Once);
			}
		}

		public class AndTheEventCodeIsARestEventCode
		{
			[Theory, StartingStateHasTransitionAttribute]
			public void ItShouldTransitionToTheStartState(IFixture fixture, IStateMachine stateMachine, string eventCode)
			{
				// Arrange
				A.CallTo(() => stateMachine.IsResetEvent(eventCode)).Returns(true);

				var startingState = stateMachine.StartingState;

				var sut = fixture.Create<Controller>();

				sut.HandleEventCode(eventCode);

				// Act
				sut.HandleEventCode(eventCode);

				// Assert
				sut.CurrentState.Should().BeSameAs(startingState);
			}

			[Theory, StartingStateHasTransitionAttribute]
			public void ItShouldExecuteTheCommandsOnTheStartState(IFixture fixture, IStateMachine stateMachine, [Frozen]ICommandChannel commandChannel, string eventCode)
			{
				// Arrange
				A.CallTo(() => stateMachine.IsResetEvent(eventCode)).Returns(true);

				var startingState = stateMachine.StartingState;

				var sut = fixture.Create<Controller>();

				sut.HandleEventCode(eventCode);

				// Act
				sut.HandleEventCode(eventCode);

				// Assert
				A.CallTo(() => startingState.ExecuteActions(commandChannel)).MustHaveHappened(Repeated.Exactly.Once);
			}
		}

		public class AndTheEventCodeIsNotRecognized
		{
			[Theory, AutoFakeItEasyData]
			public void ItShouldNotChangeTheCurrentState(IFixture fixture,  string eventCode)
			{
				// Arrange
				var sut = fixture.Create<Controller>();

				var expected = sut.CurrentState;

				// Act
				sut.HandleEventCode(eventCode);

				// Assert
				sut.CurrentState.Should().BeSameAs(expected);
			}

			[Theory, AutoFakeItEasyData]
			public void ItShouldNotExecuteTheCommandsOnTheCurrentState(IFixture fixture, string eventCode)
			{
				// Arrange
				var sut = fixture.Create<Controller>();

				var currentState = sut.CurrentState;

				// Act
				sut.HandleEventCode(eventCode);

				// Assert
				A.CallTo(() => currentState.ExecuteActions(A<ICommandChannel>._)).MustNotHaveHappened();
			}
		}
	}
}