using DSLExamples.SemanticModel;

namespace DSLExamples.UnitTests.IntegrationTests.CommandQueryApi
{
	public class SecretPanelSemanticModel
	{
		readonly Event _doorClosed;
		readonly Event _doorOpened;
		readonly Event _lightOn;
		readonly Event _drawerOpened;
		readonly Event _panelClosed;

		readonly Command _lockPanelCmd;
		readonly Command _unlockPanelCmd;
		readonly Command _lockDoorCmd;
		readonly Command _unlockDoorCmd;

		readonly State _idleState;
		readonly State _activeState;
		readonly State _waitingForLightState;
		readonly State _waitingForDrawerState;
		readonly State _unlockedPanelState;


		public Event DoorClosed { get { return _doorClosed; } }

		public Event DoorOpened { get { return _doorOpened; } }

		public Event LightOn { get { return _lightOn; } }

		public Event DrawerOpened { get { return _drawerOpened; } }

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


		public SecretPanelSemanticModel()
		{
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

		public IStateMachine CreateStateMachine()
		{
			var stateMachine = new StateMachine(_idleState);
			stateMachine.AddResetEvent(_doorOpened);

			return stateMachine;
		}
	}
}