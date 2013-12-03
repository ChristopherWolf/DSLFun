using System.Linq;
using Common.Specifications;
using Common.UnitTests.TestingHelpers;
using FakeItEasy;
using FluentAssertions;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Xunit;
using Xunit.Extensions;

// ReSharper disable CheckNamespace
namespace Common.UnitTests.Specifications.NotSpecificationTests
// ReSharper restore CheckNamespace
{
	public class WhenVerifyingArchitecturalConstraints
	{
		[Theory, AutoFakeItEasyData]
		public void AllConstructorArgumentsShouldBeExposedAsWellBehavedReadOnlyProperties(IFixture fixture)
		{
			// Arrange
			var assertion = new ConstructorInitializedMemberAssertion(fixture);
			var type = typeof(NotSpecification<TestType>);

			// Act
			var constructors = type.GetConstructors();
			var readOnlyProperties = type.GetProperties().Where(x => x.GetSetMethod(nonPublic: true) == null);

			// Assert
			assertion.Verify(constructors);
			assertion.Verify(readOnlyProperties);
		}

		[Theory, AutoFakeItEasyData]
		public void ItShouldBeASpecification(NotSpecification<TestType> sut)
		{
			sut.Should().BeAssignableTo<ISpecification<TestType>>();
		}
	}

	public class WhenTestingIfSpecificationIsCorrect
	{
		[Theory, AutoFakeItEasyData]
		public void ItShouldReturnTrueIfInnerSpecificationReturnsFalse(IFixture fixture, [Frozen]ISpecification<TestType> inner, TestType item)
		{
			// Arrange
			A.CallTo(() => inner.IsSatisfiedBy(item)).Returns(false);

			var sut = fixture.Create<NotSpecification<TestType>>();

			// Act
			var result = sut.IsSatisfiedBy(item);

			// Assert
			result.Should().BeTrue();
		}

		[Theory, AutoFakeItEasyData]
		public void ItShouldReturnFalseIfInnerSpecificationReturnsTrue(IFixture fixture, [Frozen]ISpecification<TestType> inner, TestType item)
		{
			// Arrange
			A.CallTo(() => inner.IsSatisfiedBy(item)).Returns(true);

			var sut = fixture.Create<NotSpecification<TestType>>();

			// Act
			var result = sut.IsSatisfiedBy(item);

			// Assert
			result.Should().BeFalse();
		} 
	}
}