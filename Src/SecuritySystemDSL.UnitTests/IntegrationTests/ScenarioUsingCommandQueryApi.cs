using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using FluentAssertions;
using SecuritySystemDSL.SemanticModel;
using Xunit;

namespace SecuritySystemDSL.UnitTests.IntegrationTests
{
	public class HistoryRecordingCommandChannel : ICommandChannel
	{
		readonly IList<string> _eventCodeHistory;

		public IEnumerable<string> EventCodeHistory { get { return _eventCodeHistory.Repeat(); } }

		public HistoryRecordingCommandChannel()
		{
			_eventCodeHistory = new List<string>();
		}

		#region ICommandChannel Members

		public void Send(string eventCode)
		{
			if (eventCode == null) throw new ArgumentNullException("eventCode");

			string message = string.Format("Command channel received event code: {0}", eventCode);

			Console.WriteLine(message);

			_eventCodeHistory.Add(eventCode);
		}

		#endregion
	}

	public class ScenarioData
	{
		readonly State _activeState;
		readonly HistoryRecordingCommandChannel _commandChannel;
		readonly Controller _controller;

		readonly Event _doorClosed;
		readonly Event _doorOpened;
		readonly Event _drawerOpened;
		readonly State _idleState;
		readonly Event _lightOn;
		readonly Command _lockDoorCmd;
		readonly Command _lockPanelCmd;
		readonly Event _panelClosed;
		readonly Command _unlockDoorCmd;
		readonly Command _unlockPanelCmd;

		readonly State _unlockedPanelState;
		readonly State _waitingForDrawerState;
		readonly State _waitingForLightState;

		public HistoryRecordingCommandChannel CommandChannel { get { return _commandChannel; } }

		public Controller Controller { get { return _controller; } }

		public Event DoorOpened { get { return _doorOpened; } }

		public Event DoorClosed { get { return _doorClosed; } }

		public Event DrawerOpened { get { return _drawerOpened; } }

		public Event LightOn { get { return _lightOn; } }

		public Event PanelClosed { get { return _panelClosed; } }

		public Command LockPanelCmd { get { return _lockPanelCmd; } }

		public Command UnlockPanelCmd { get { return _unlockPanelCmd; } }

		public Command LockDoorCmd { get { return _lockDoorCmd; } }

		public Command UnlockDoorCmd { get { return _unlockDoorCmd; } }

		public State IdleState { get { return _idleState; } }

		public State ActiveState { get { return _activeState; } }

		public State WaitingForLightState { get { return _waitingForLightState; } }

		public State WaitingForDrawerState { get { return _waitingForDrawerState; } }

		public State UnlockedPanelState { get { return _unlockedPanelState; } }

		public ScenarioData()
		{
			_commandChannel = new HistoryRecordingCommandChannel();

			_doorOpened = new Event("DoorOpened", "D1OP");
			_doorClosed = new Event("DoorClosed", "D1CL");
			_drawerOpened = new Event("DrawerOpened", "D2OP");
			_lightOn = new Event("LightOn", "L1ON");
			_panelClosed = new Event("PanelClosed", "PNCL");

			_lockPanelCmd = new Command("LockPanel", "PNLK");
			_unlockPanelCmd = new Command("UnlockPanel", "PNUL");
			_lockDoorCmd = new Command("LockDoor", "D1LK");
			_unlockDoorCmd = new Command("UnlockDoor", "D1UL");

			_idleState = new State("Idle");
			_activeState = new State("Active");
			_waitingForLightState = new State("WaitingForLight");
			_waitingForDrawerState = new State("WaitingForDrawer");
			_unlockedPanelState = new State("UnlockedPanel");

			SetupTransitions();

			_controller = CreateController();
		}

		void SetupTransitions()
		{
			_idleState.AddTransition(_doorClosed, _activeState);
			_idleState.AddAction(_unlockDoorCmd);
			_idleState.AddAction(_lockPanelCmd);

			_activeState.AddTransition(_drawerOpened, _waitingForLightState);
			_activeState.AddTransition(_lightOn, _waitingForDrawerState);

			_waitingForLightState.AddTransition(_lightOn, _unlockedPanelState);

			_waitingForDrawerState.AddTransition(_drawerOpened, _unlockedPanelState);

			_unlockedPanelState.AddTransition(_panelClosed, _idleState);
			_unlockedPanelState.AddAction(_unlockPanelCmd);
			_unlockedPanelState.AddAction(_lockDoorCmd);
		}

		Controller CreateController()
		{
			var stateMachine = new StateMachine(_idleState);
			stateMachine.AddResetEvent(_doorOpened);

			return new Controller(stateMachine, _commandChannel);
		}
	}

	public class ScenarioUsingCommandQueryApi
	{
		readonly ScenarioData _scenarioData;

		public ScenarioUsingCommandQueryApi()
		{
			_scenarioData = new ScenarioData();
		}

		IEnumerable<Event> GetCodesToUnlockPanelViaRouteA()
		{
			yield return _scenarioData.DoorClosed;
			yield return _scenarioData.LightOn;
			yield return _scenarioData.DrawerOpened;
		}

		IEnumerable<Event> GetCodesToUnlockPanelViaRouteB()
		{
			yield return _scenarioData.DoorClosed;
			yield return _scenarioData.DrawerOpened;
			yield return _scenarioData.LightOn;
		}

		IEnumerable<Event> GetCodesForReset()
		{
			yield return _scenarioData.DoorClosed;
			yield return _scenarioData.LightOn;
			yield return _scenarioData.DoorOpened;
		}

		[Fact]
		public void UnlockPanelUsingRouteA()
		{
			// Arrange
			Controller controller = _scenarioData.Controller;
			List<Event> codes = GetCodesToUnlockPanelViaRouteA().ToList();

			// Act
			codes.ForEach(x => controller.HandleEventCode(x.Code));

			// Assert
			controller.CurrentState.Should().Be(_scenarioData.UnlockedPanelState);
		}

		[Fact]
		public void CommandsforUnlockPanelStateUnlockPanelUsingRouteA()
		{
			// Arrange
			Controller controller = _scenarioData.Controller;
			List<Event> codes = GetCodesToUnlockPanelViaRouteA().ToList();

			List<string> expected = new[] {_scenarioData.UnlockPanelCmd.Code, _scenarioData.LockDoorCmd.Code}.ToList();

			// Act
			codes.ForEach(x => controller.HandleEventCode(x.Code));

			// Assert
			_scenarioData.CommandChannel.EventCodeHistory.Should().Equal(expected);
		}

		[Fact]
		public void UnlockPanelUsingRouteB()
		{
			// Arrange
			Controller controller = _scenarioData.Controller;

			List<Event> codes = GetCodesToUnlockPanelViaRouteB().ToList();

			// Act
			codes.ForEach(x => controller.HandleEventCode(x.Code));

			// Assert
			controller.CurrentState.Should().Be(_scenarioData.UnlockedPanelState);
		}

		[Fact]
		public void CommandsforUnlockPanelStateUnlockPanelUsingRouteB()
		{
			// Arrange
			Controller controller = _scenarioData.Controller;
			List<Event> codes = GetCodesToUnlockPanelViaRouteB().ToList();

			List<string> expected = new[] {_scenarioData.UnlockPanelCmd.Code, _scenarioData.LockDoorCmd.Code}.ToList();

			// Act
			codes.ForEach(x => controller.HandleEventCode(x.Code));

			// Assert
			_scenarioData.CommandChannel.EventCodeHistory.Should().Equal(expected);
		}

		[Fact]
		public void SendResetEvent()
		{
			// Arrange
			Controller controller = _scenarioData.Controller;

			List<Event> codes = GetCodesForReset().ToList();

			// Act
			codes.ForEach(x => controller.HandleEventCode(x.Code));

			// Assert
			controller.CurrentState.Should().Be(_scenarioData.IdleState);
		}

		[Fact]
		public void CommandsforIdleState()
		{
			// Arrange
			Controller controller = _scenarioData.Controller;

			List<string> expected = new[] {_scenarioData.UnlockDoorCmd.Code, _scenarioData.LockPanelCmd.Code}.ToList();

			// Act
			controller.HandleEventCode(_scenarioData.DoorClosed.Code);
			controller.HandleEventCode(_scenarioData.DoorOpened.Code);

			// Assert
			_scenarioData.CommandChannel.EventCodeHistory.Should().Equal(expected);
		}
	}
}