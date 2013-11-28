using Common.UnitTests.TestingHelpers;
using FakeItEasy;
using Ploeh.AutoFixture;
using SecuritySystemDSL.SemanticModel;
using Xunit;
using Xunit.Extensions;

namespace SecuritySystemDSL.UnitTests.IntegrationTests
{
	public class ScenarioUsingCommandQueryApi
	{
		Controller CreateController()
		{
			var doorClosed = new Event("doorClosed", "D1CL");
			var drawerOpened = new Event("drawerOpened", "D2OP");
			var lightOn = new Event("lightOn", "L1ON");
			var doorOpened = new Event("doorOpened", "D1OP");
			var panelClosed = new Event("panelClosed", "PNCL");

			var unlockPanelCmd = new Command("unlockPanelCmd", "PNUL");
			var lockPanelCmd = new Command("lockPanelCmd", "PNLK");
			var lockDoorCmd = new Command("lockDoorCmd", "D1LK");
			var unlockDoorCmd = new Command("unlockDoorCmd", "D1UL");

			var idleState = new State("idle");
			var activeState = new State("active");
			var waitingForLightState = new State("waitingForLight");
			var waitingForDrawerState = new State("waitingForDrawer");
			var unlockedPanelState = new State("unlockedPanel");

			var stateMachine = new StateMachine(null);

			var commandChannel = new ConsoleCommandChannel();

			return new Controller(stateMachine, commandChannel);
		}

		[Fact]
		public void SecretPanelTest()
		{
			// Arrange
			var controller = CreateController();

			// Act

			// Assert

		}
	}
}