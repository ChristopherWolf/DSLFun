using System;
using System.Linq;
using Common.UnitTests.TestingHelpers;
using DSLExamples.SecuritySystem.SemanticModel;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Idioms;
using Xunit.Extensions;

// ReSharper disable CheckNamespace
namespace DSLExamples.SecuritySystem.SecuritySystem.UnitTests.SemanticModel.TransitionTests
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
	}
}