using System;
using System.Collections.Generic;
using System.Linq;
using Common.UnitTests.TestingHelpers;
using FluentAssertions;
using Ploeh.AutoFixture;
using DSLExamples.SemanticModel;
using Xunit.Extensions;

namespace DSLExamples.UnitTests.IntegrationTests.CommandQueryApi
{
	#region Customizations

	internal class ScenarioUsingCommandQueryApiCustomization : ICustomization
	{
		public void Customize(IFixture fixture)
		{
			var commandChannel = fixture.Freeze<HistoryRecordingCommandChannel>();

			var model = fixture.Freeze<SecretPanelSemanticModel>();

			fixture.Register(() => new Controller(model.CreateStateMachine(), commandChannel));
		}
	}

	public class ScenarioUsingCommandQueryApiAttribute : AutoFakeItEasyDataAttribute
	{
		public ScenarioUsingCommandQueryApiAttribute()
			: base(new ScenarioUsingCommandQueryApiCustomization())
		{
		}
	}

	#endregion

	#region Test Helpers

	public class EventCodeSequences
	{
		readonly SecretPanelSemanticModel _semanticModel;

		public EventCodeSequences(SecretPanelSemanticModel semanticModel)
		{
			if (semanticModel == null) throw new ArgumentNullException("semanticModel");

			_semanticModel = semanticModel;
		}

		public IEnumerable<Event> EventSequenceToUnlockPanelViaRouteA
		{
			get
			{
				yield return _semanticModel.DoorClosed;
				yield return _semanticModel.LightOn;
				yield return _semanticModel.DrawerOpened;
			}
		}

		public IEnumerable<Event> EventSequenceToUnlockPanelViaRouteB
		{
			get
			{
				yield return _semanticModel.DoorClosed;
				yield return _semanticModel.DrawerOpened;
				yield return _semanticModel.LightOn;
			}
		}

		public IEnumerable<Event> EventSequenceWithReset
		{
			get
			{
				yield return _semanticModel.DoorClosed;
				yield return _semanticModel.LightOn;
				yield return _semanticModel.DoorOpened;
			}
		}
	}

	#endregion

	public class ScenarioUsingCommandQueryApi
	{
		[Theory, ScenarioUsingCommandQueryApi]
		public void UnlockPanelViaRouteA(Controller controller, SecretPanelSemanticModel semanticModel, EventCodeSequences sequences)
		{
			// Arrange
			var codes = sequences.EventSequenceToUnlockPanelViaRouteA.ToList();

			// Act
			codes.ForEach(x => controller.HandleEventCode(x.Code));

			// Assert
			controller.CurrentState.Should().Be(semanticModel.UnlockedPanelState);
		}

		[Theory, ScenarioUsingCommandQueryApi]
		public void CheckCommandCodesWhenUnlockingPanelViaRouteA(Controller controller, SecretPanelSemanticModel semanticModel, HistoryRecordingCommandChannel commandChannel, EventCodeSequences sequences)
		{
			// Arrange
			var codes = sequences.EventSequenceToUnlockPanelViaRouteA.ToList();

			var expected = new[] { semanticModel.UnlockPanelCmd.Code, semanticModel.LockDoorCmd.Code }.ToList();

			// Act
			codes.ForEach(x => controller.HandleEventCode(x.Code));

			// Assert
			commandChannel.EventCodeHistory.Should().Equal(expected);
		}

		[Theory, ScenarioUsingCommandQueryApi]
		public void UnlockPanelViaRouteB(Controller controller, SecretPanelSemanticModel semanticModel, EventCodeSequences sequences)
		{
			// Arrange
			var codes = sequences.EventSequenceToUnlockPanelViaRouteB.ToList();

			// Act
			codes.ForEach(x => controller.HandleEventCode(x.Code));

			// Assert
			controller.CurrentState.Should().Be(semanticModel.UnlockedPanelState);
		}

		[Theory, ScenarioUsingCommandQueryApi]
		public void CheckCommandCodesWhenUnlockingPanelViaRouteB(Controller controller, SecretPanelSemanticModel semanticModel, HistoryRecordingCommandChannel commandChannel, EventCodeSequences sequences)
		{
			// Arrange
			var codes = sequences.EventSequenceToUnlockPanelViaRouteB.ToList();

			var expected = new[] { semanticModel.UnlockPanelCmd.Code, semanticModel.LockDoorCmd.Code }.ToList();

			// Act
			codes.ForEach(x => controller.HandleEventCode(x.Code));

			// Assert
			commandChannel.EventCodeHistory.Should().Equal(expected);
		}


		[Theory, ScenarioUsingCommandQueryApi]
		public void SendResetEventAfterMovingToANewState(Controller controller, SecretPanelSemanticModel semanticModel, EventCodeSequences sequences)
		{
			// Arrange
			var codes = sequences.EventSequenceWithReset.ToList();

			// Act
			codes.ForEach(x => controller.HandleEventCode(x.Code));

			// Assert
			controller.CurrentState.Should().Be(semanticModel.IdleState);
		}

		[Theory, ScenarioUsingCommandQueryApi]
		public void CheckCommandCodesWhenResettingToInitialState(Controller controller, SecretPanelSemanticModel semanticModel, HistoryRecordingCommandChannel commandChannel, EventCodeSequences sequences)
		{
			// Arrange
			var expected = new[] { semanticModel.UnlockDoorCmd.Code, semanticModel.LockPanelCmd.Code }.ToList();

			// Act
			controller.HandleEventCode(semanticModel.DoorClosed.Code);
			controller.HandleEventCode(semanticModel.DoorOpened.Code);

			// Assert
			commandChannel.EventCodeHistory.Should().Equal(expected);
		}
	}
}