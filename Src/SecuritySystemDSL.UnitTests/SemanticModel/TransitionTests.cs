using System;
using System.Linq;
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
namespace SecuritySystemDSL.UnitTests.SemanticModel.TransitionTests
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
			Type sutType = typeof(Transition);

			// Assert
			assertion.Verify(sutType);
		}

		[Theory, AutoFakeItEasyData]
		public void AllConstructorArgumentsShouldBeExposedAsWellBehavedReadOnlyProperties(IFixture fixture)
		{
			// Arrange
			var assertion = new ConstructorInitializedMemberAssertion(fixture);
			var type = typeof(Transition);

			// Act
			var constructors = type.GetConstructors();
			var readOnlyProperties = type.GetProperties().Where(x => x.GetSetMethod(nonPublic: true) == null && x.Name != "EventCode");

			// Assert
			assertion.Verify(constructors);
			assertion.Verify(readOnlyProperties);
		}

		[Theory, AutoFakeItEasyData]
		public void TheEventCodeShouldBeCorrect(IFixture fixture, [Frozen]Event trigger)
		{
			// Arrange
			var expected = trigger.Code;

			var sut = fixture.Create<Transition>();

			// Act
			var result = sut.EventCode;

			// Assert
			result.Should().Be(expected);
		}
	}
}