using Common.Specifications;
using Common.UnitTests.Specifications;
using Common.UnitTests.TestingHelpers;
using FluentAssertions;
using Ploeh.AutoFixture;
using Xunit.Extensions;

// ReSharper disable CheckNamespace
namespace Common.UnitTests.EnumerableExtensionsTests.Specifications.AndSpecificationTests
// ReSharper restore CheckNamespace
{
	public class WhenVerifyingArchitecturalConstraints
	{
		[Theory, AutoFakeItEasyData]
		public void ItShouldBeASpecification(IFixture fixture)
		{
			// Arrange

			// Act
			var sut = fixture.Create<AndSpecification<TestType>>();

			// Assert
			sut.Should().BeAssignableTo<ISpecification<TestType>>();
		}
	}

	public class WhenTestingIfSpecificationIsCorrect
	{
		[Theory, AllInnerSpecificationsPass]
		public void ItShouldReturnTrueIfAllInnerSpecificationsAreSatisfied(IFixture fixture, TestType item)
		{
			// Arrange
			var sut = fixture.Create<AndSpecification<TestType>>();

			// Act
			var result = sut.IsSatisfiedBy(item);

			// Assert
			result.Should().BeTrue();
		}

		[Theory, AllInnerSpecificationsFail]
		public void ItShouldReturnFalseIfAllInnerSpecificationsAreNotSatisfied(IFixture fixture, TestType item)
		{
			// Arrange
			var sut = fixture.Create<AndSpecification<TestType>>();

			// Act
			var result = sut.IsSatisfiedBy(item);

			// Assert
			result.Should().BeFalse();
		}

		[Theory, SpecificationsAreMixed]
		public void ItShouldReturnFalseIfSomeInnerSpecificationsAreNotSatisfied(IFixture fixture, TestType item)
		{
			// Arrange
			var sut = fixture.Create<AndSpecification<TestType>>();

			// Act
			var result = sut.IsSatisfiedBy(item);

			// Assert
			result.Should().BeFalse();
		} 
	}
}