using System.Linq;
using Common.UnitTests.TestingHelpers;
using DSLExamples.RecurringEvents.SemanticModel;
using FakeItEasy;
using FluentAssertions;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Idioms;
using Xunit.Extensions;

// ReSharper disable CheckNamespace
namespace DSLExamples.UnitTests.RecurringEvents.SemanticModel.OrSpecificationTests
// ReSharper restore CheckNamespace
{
	public class WhenVerifyingArchitecturalConstraints
	{
		[Theory, AutoFakeItEasyData]
		public void ItShouldBeASpecification(OrSpecification<TestType> sut)
		{
			sut.Should().BeAssignableTo<ISpecification<TestType>>();
		}

		[Theory, AutoFakeItEasyData]
		public void AllConstructorArgumentsShouldBeExposedAsWellBehavedReadOnlyProperties(IFixture fixture)
		{
			// Arrange
			var assertion = new ConstructorInitializedMemberAssertion(fixture);
			var type = typeof(OrSpecification<TestType>);

			// Act
			var constructors = type.GetConstructors();
			var readOnlyProperties = type.GetProperties().Where(x => x.GetSetMethod(nonPublic: true) == null);

			// Assert
			assertion.Verify(constructors);
			assertion.Verify(readOnlyProperties);
		}
	}

	public class WhenTestingIfSpecificationIsCorrect
	{
		[Theory]
		[InlineData(true, true, true)]
		[InlineData(true, false, true)]
		[InlineData(false, true, true)]
		[InlineData(false, false, false)]
		public void ItShouldReturnTheCorrectValueForAllPossibleLHSAndRHSCombinations(bool lhsResult, bool rhsResult, bool expected)
		{
			// Arrange
			var item = new TestType();

			var lhs = A.Fake<ISpecification<TestType>>();
			A.CallTo(() => lhs.IsSatisfiedBy(item)).Returns(lhsResult);

			var rhs = A.Fake<ISpecification<TestType>>();
			A.CallTo(() => rhs.IsSatisfiedBy(item)).Returns(rhsResult);

			var sut = new OrSpecification<TestType>(lhs, rhs);

			// Act
			var result = sut.IsSatisfiedBy(item);

			// Assert
			result.Should().Be(expected);
		}
	}
}