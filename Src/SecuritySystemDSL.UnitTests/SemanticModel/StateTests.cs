using System;
using System.Linq;
using FakeItEasy;
using FluentAssertions;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoFakeItEasy;
using Ploeh.AutoFixture.Idioms;
using Ploeh.SemanticComparison.Fluent;
using SecuritySystemDSL.SemanticModel;
using Xunit;
using Xunit.Extensions;

// ReSharper disable CheckNamespace
namespace SecuritySystemDSL.UnitTests.SemanticModel.StateTests
// ReSharper restore CheckNamespace
{
//	#region Customizations
//
//	internal class TestCustomization : ICustomization
//	{
//		public void Customize(IFixture fixture) {}
//	}
//
//	#endregion
//
//	#region Test Doubles
//
//	#endregion

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
			var readOnlyProperties = type.GetProperties().Where(x => x.GetSetMethod(nonPublic: true) == null);

			// Assert
			assertion.Verify(constructors);
			assertion.Verify(readOnlyProperties);
		}
	}
}