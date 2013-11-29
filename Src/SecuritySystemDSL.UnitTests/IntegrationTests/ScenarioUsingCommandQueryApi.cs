using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using SecuritySystemDSL.SemanticModel;
using Xunit;

namespace SecuritySystemDSL.UnitTests.IntegrationTests
{
	public class ScenarioUsingCommandQueryApi
	{
		readonly SecretPanelStateMachineData _secretPanelStateMachineData;

		public ScenarioUsingCommandQueryApi()
		{
			_secretPanelStateMachineData = new SecretPanelStateMachineData();
		}

		IEnumerable<Event> GetCodesToUnlockPanelViaRouteA()
		{
			yield return _secretPanelStateMachineData.DoorClosed;
			yield return _secretPanelStateMachineData.LightOn;
			yield return _secretPanelStateMachineData.DrawerOpened;
		}

		IEnumerable<Event> GetCodesToUnlockPanelViaRouteB()
		{
			yield return _secretPanelStateMachineData.DoorClosed;
			yield return _secretPanelStateMachineData.DrawerOpened;
			yield return _secretPanelStateMachineData.LightOn;
		}

		IEnumerable<Event> GetCodesForReset()
		{
			yield return _secretPanelStateMachineData.DoorClosed;
			yield return _secretPanelStateMachineData.LightOn;
			yield return _secretPanelStateMachineData.DoorOpened;
		}

		[Fact]
		public void UnlockPanelUsingRouteA()
		{
			// Arrange
			Controller controller = _secretPanelStateMachineData.Controller;
			List<Event> codes = GetCodesToUnlockPanelViaRouteA().ToList();

			// Act
			codes.ForEach(x => controller.HandleEventCode(x.Code));

			// Assert
			controller.CurrentState.Should().Be(_secretPanelStateMachineData.UnlockedPanelState);
		}

		[Fact]
		public void CommandsforUnlockPanelStateUnlockPanelUsingRouteA()
		{
			// Arrange
			Controller controller = _secretPanelStateMachineData.Controller;
			List<Event> codes = GetCodesToUnlockPanelViaRouteA().ToList();

			List<string> expected = new[] {_secretPanelStateMachineData.UnlockPanelCmd.Code, _secretPanelStateMachineData.LockDoorCmd.Code}.ToList();

			// Act
			codes.ForEach(x => controller.HandleEventCode(x.Code));

			// Assert
			_secretPanelStateMachineData.CommandChannel.EventCodeHistory.Should().Equal(expected);
		}

		[Fact]
		public void UnlockPanelUsingRouteB()
		{
			// Arrange
			Controller controller = _secretPanelStateMachineData.Controller;

			List<Event> codes = GetCodesToUnlockPanelViaRouteB().ToList();

			// Act
			codes.ForEach(x => controller.HandleEventCode(x.Code));

			// Assert
			controller.CurrentState.Should().Be(_secretPanelStateMachineData.UnlockedPanelState);
		}

		[Fact]
		public void CommandsforUnlockPanelStateUnlockPanelUsingRouteB()
		{
			// Arrange
			Controller controller = _secretPanelStateMachineData.Controller;
			List<Event> codes = GetCodesToUnlockPanelViaRouteB().ToList();

			List<string> expected = new[] {_secretPanelStateMachineData.UnlockPanelCmd.Code, _secretPanelStateMachineData.LockDoorCmd.Code}.ToList();

			// Act
			codes.ForEach(x => controller.HandleEventCode(x.Code));

			// Assert
			_secretPanelStateMachineData.CommandChannel.EventCodeHistory.Should().Equal(expected);
		}

		[Fact]
		public void SendResetEvent()
		{
			// Arrange
			Controller controller = _secretPanelStateMachineData.Controller;

			List<Event> codes = GetCodesForReset().ToList();

			// Act
			codes.ForEach(x => controller.HandleEventCode(x.Code));

			// Assert
			controller.CurrentState.Should().Be(_secretPanelStateMachineData.IdleState);
		}

		[Fact]
		public void CommandsforIdleState()
		{
			// Arrange
			Controller controller = _secretPanelStateMachineData.Controller;

			List<string> expected = new[] {_secretPanelStateMachineData.UnlockDoorCmd.Code, _secretPanelStateMachineData.LockPanelCmd.Code}.ToList();

			// Act
			controller.HandleEventCode(_secretPanelStateMachineData.DoorClosed.Code);
			controller.HandleEventCode(_secretPanelStateMachineData.DoorOpened.Code);

			// Assert
			_secretPanelStateMachineData.CommandChannel.EventCodeHistory.Should().Equal(expected);
		}
	}
}