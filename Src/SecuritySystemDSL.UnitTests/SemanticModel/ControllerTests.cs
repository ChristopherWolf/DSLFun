using System;
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

namespace SecuritySystemDSL.UnitTests.SemanticModel.ControllerTests
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
			Type sutType = typeof(Controller);

			// Assert
			assertion.Verify(sutType);
		}
	}
}