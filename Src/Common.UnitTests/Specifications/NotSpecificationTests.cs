using Common.Specifications;
using Common.UnitTests.TestingHelpers;
using FakeItEasy;
using FluentAssertions;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;
using Xunit.Extensions;

// ReSharper disable CheckNamespace
namespace Common.UnitTests.Specifications.NotSpecificationTests
// ReSharper restore CheckNamespace
{
	public class WhenVerifyingArchitecturalConstraints
	{
		[Theory, AutoFakeItEasyData]
		public void ItShouldImplementTheExpectedRoles(IFixture fixture)
		{
			// Arrange
			// Act
			var sut = fixture.Create<NotSpecification<TestType>>();

			// Assert
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