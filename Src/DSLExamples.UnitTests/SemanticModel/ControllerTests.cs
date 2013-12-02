using System;
using Common.UnitTests.TestingHelpers;
using FakeItEasy;
using FluentAssertions;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Xunit;
using DSLExamples.SemanticModel;
using Xunit.Extensions;

// ReSharper disable CheckNamespace
namespace DSLExamples.UnitTests.SemanticModel.ControllerTests
// ReSharper restore CheckNamespace
{
	#region Customizations

	internal class StartingStateHasTransitionCustomization : ICustomization
	{
		public void Customize(IFixture fixture)
		{
			var eventCodes = fixture.Freeze<EventCodes>();

			var startState = fixture.Create<IState>();
			A.CallTo(() => startState.ToString()).Returns("Starting state");

			A.CallTo(() => startState.HasTransition(eventCodes.NewStateEventCode)).Returns(true);

			var newState = fixture.Freeze<IState>();
			A.CallTo(() => newState.ToString()).Returns("New state");

			A.CallTo(() => startState.FindTargetState(eventCodes.NewStateEventCode)).Returns(newState);

			var stateMachine = fixture.Freeze<IStateMachine>();
			A.CallTo(() => stateMachine.StartingState).Returns(startState);
			A.CallTo(() => stateMachine.IsResetEvent(eventCodes.ResetEventCode)).Returns(true);
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

	#region Test Doubles

	public class EventCodes
	{
		readonly string _newStateEventCode;
		readonly string _resetEventCode;

		public EventCodes(string newStateEventCode, string resetEventCode)
		{
			if (newStateEventCode == null) throw new ArgumentNullException("newStateEventCode");
			if (resetEventCode == null) throw new ArgumentNullException("resetEventCode");

			_newStateEventCode = newStateEventCode;
			_resetEventCode = resetEventCode;
		}

		public string NewStateEventCode { get { return _newStateEventCode; } }

		public string ResetEventCode { get { return _resetEventCode; } }
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
			public void ItShouldTransitionToTheNewState(IStateMachine stateMachine, IState newState, EventCodes eventCodes, Controller sut)
			{
				// Arrange

				// Act
				sut.HandleEventCode(eventCodes.NewStateEventCode);

				// Assert
				sut.CurrentState.Should().BeSameAs(newState);
			}

			[Theory, StartingStateHasTransition]
			public void ItShouldExecuteTheCommandsOnTheNewState(IState newState, [Frozen]ICommandChannel commandChannel, EventCodes eventCodes, Controller sut)
			{
				// Arrange

				// Act
				sut.HandleEventCode(eventCodes.NewStateEventCode);

				// Assert
				A.CallTo(() => newState.ExecuteActions(commandChannel)).MustHaveHappened(Repeated.Exactly.Once);
			}
		}

		public class AndTheEventCodeIsARestEventCode
		{
			[Theory, StartingStateHasTransition]
			public void ItShouldTransitionToTheStartState(IStateMachine stateMachine, EventCodes eventCodes, Controller sut)
			{
				// Arrange
				var startingState = stateMachine.StartingState;

				sut.HandleEventCode(eventCodes.NewStateEventCode);

				// Act
				sut.HandleEventCode(eventCodes.ResetEventCode);

				// Assert
				sut.CurrentState.Should().BeSameAs(startingState);
			}

			[Theory, StartingStateHasTransition]
			public void ItShouldExecuteTheCommandsOnTheStartState(IStateMachine stateMachine, [Frozen]ICommandChannel commandChannel, EventCodes eventCodes, Controller sut)
			{
				// Arrange
				var startingState = stateMachine.StartingState;

				sut.HandleEventCode(eventCodes.NewStateEventCode);

				// Act
				sut.HandleEventCode(eventCodes.ResetEventCode);

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