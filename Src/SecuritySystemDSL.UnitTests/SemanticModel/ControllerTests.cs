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
		public void CurrentStateShouldBeInitializedToStateMachinesStartingState([Frozen] IStateMachine stateMachine, Controller sut)
		{
			// Arrange
			var expected = stateMachine.StartingState;

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
			public void ItShouldTransitionToTheNewState(IStateMachine stateMachine, IState newState, string eventCode, Controller sut)
			{
				// Arrange

				// Act
				sut.HandleEventCode(eventCode);

				// Assert
				sut.CurrentState.Should().BeSameAs(newState);
			}

			[Theory, StartingStateHasTransition]
			public void ItShouldExecuteTheCommandsOnTheNewState(IState newState, [Frozen]ICommandChannel commandChannel, string eventCode, Controller sut)
			{
				// Arrange

				// Act
				sut.HandleEventCode(eventCode);

				// Assert
				A.CallTo(() => newState.ExecuteActions(commandChannel)).MustHaveHappened(Repeated.Exactly.Once);
			}
		}

		public class AndTheEventCodeIsARestEventCode
		{
			[Theory, StartingStateHasTransitionAttribute]
			public void ItShouldTransitionToTheStartState(IStateMachine stateMachine, string eventCode, Controller sut)
			{
				// Arrange
				A.CallTo(() => stateMachine.IsResetEvent(eventCode)).Returns(true);

				var startingState = stateMachine.StartingState;

				sut.HandleEventCode(eventCode);

				// Act
				sut.HandleEventCode(eventCode);

				// Assert
				sut.CurrentState.Should().BeSameAs(startingState);
			}

			[Theory, StartingStateHasTransitionAttribute]
			public void ItShouldExecuteTheCommandsOnTheStartState(IStateMachine stateMachine, [Frozen]ICommandChannel commandChannel, string eventCode, Controller sut)
			{
				// Arrange
				A.CallTo(() => stateMachine.IsResetEvent(eventCode)).Returns(true);

				var startingState = stateMachine.StartingState;

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
			public void ItShouldNotChangeTheCurrentState(string eventCode, Controller sut)
			{
				// Arrange
				var expected = sut.CurrentState;

				// Act
				sut.HandleEventCode(eventCode);

				// Assert
				sut.CurrentState.Should().BeSameAs(expected);
			}

			[Theory, AutoFakeItEasyData]
			public void ItShouldNotExecuteTheCommandsOnTheCurrentState(string eventCode, Controller sut)
			{
				// Arrange
				var currentState = sut.CurrentState;

				// Act
				sut.HandleEventCode(eventCode);

				// Assert
				A.CallTo(() => currentState.ExecuteActions(A<ICommandChannel>._)).MustNotHaveHappened();
			}
		}
	}
}