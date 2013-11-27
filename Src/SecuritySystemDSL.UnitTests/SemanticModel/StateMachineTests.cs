using System;
using System.Collections.Generic;
using System.Linq;
using Common.UnitTests.TestingHelpers;
using FakeItEasy;
using FluentAssertions;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoFakeItEasy;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Xunit;
using Ploeh.SemanticComparison.Fluent;
using SecuritySystemDSL.SemanticModel;
using Xunit.Extensions;

// ReSharper disable CheckNamespace
namespace SecuritySystemDSL.UnitTests.SemanticModel.StateMachineTests
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
			Type sutType = typeof(StateMachine);

			// Assert
			assertion.Verify(sutType);
		}

		[Theory, AutoFakeItEasyData]
		public void AllConstructorArgumentsShouldBeExposedAsWellBehavedReadOnlyProperties(IFixture fixture)
		{
			// Arrange
			var assertion = new ConstructorInitializedMemberAssertion(fixture);
			var type = typeof(StateMachine);

			// Act
			var constructors = type.GetConstructors();
			var readOnlyProperties = type.GetProperties().Where(x => x.GetSetMethod(nonPublic: true) == null && x.Name != "AllPossibleStates");

			// Assert
			assertion.Verify(constructors);
			assertion.Verify(readOnlyProperties);
		}
	}

	public class WhenGettingAllPossibleStates
	{
		[Theory, AutoFakeItEasyData]
		public void ItShouldReturnTheExpectedStates(IFixture fixture, [Frozen]IState start)
		{
			// Arrange

			fixture.Register<IState>(() =>
				{
					var newState = A.Fake<IState>();

					var target1 = A.Fake<IState>();
					var target2 = A.Fake<IState>();
					var target3 = A.Fake<IState>();

					A.CallTo(() => newState.GetAllTargets()).Returns(new[] {target1, target2, target3, target1});

					return newState;
				});

			var targets = fixture.Create<IEnumerable<IState>>().ToList();

			A.CallTo(() => start.GetAllTargets()).Returns(targets);

			var expected = start.SelectMany();

			var sut = fixture.Create<StateMachine>();

			// Act
			var result = sut.AllPossibleStates;

			// Assert
			result.Should().BeEquivalentTo(expected);
		}
	}
}