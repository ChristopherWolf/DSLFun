using System.Collections.Generic;
using Common.Specifications;
using Common.UnitTests.Specifications;
using Common.UnitTests.TestingHelpers;
using FluentAssertions;
using Ploeh.AutoFixture;
using Xunit.Extensions;

// ReSharper disable CheckNamespace
namespace Common.UnitTests.EnumerableExtensionsTests.Specifications.AndSpecificationTests.OrSpecificationTests
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
		public void InnerSpecificationsShouldBeCorrectWhenInitializedWithArray(ISpecification<TestType>[] array)
		{
			// Arrange
			var sut = new OrSpecification<TestType>(array);

			// Act
			var result = sut.InnerSpecifications;

			// Assert
			result.Should().Equal(array);
		}

		[Theory, AutoFakeItEasyData]
		public void InnerSpecificationsShouldBeCorrectWhenInitializedWithEnumerable(IEnumerable<ISpecification<TestType>> specifications)
		{
			// Arrange
			var sut = new OrSpecification<TestType>(specifications);

			// Act
			var result = sut.InnerSpecifications;

			// Assert
			result.Should().Equal(specifications);
		}
	}

	public class WhenTestingIfSpecificationIsCorrect
	{
		[Theory, AllInnerSpecificationsPass]
		public void ItShouldReturnTrueIfAllInnerSpecificationsAreSatisfied(IFixture fixture, TestType item)
		{
			// Arrange
			var sut = fixture.Create<OrSpecification<TestType>>();

			// Act
			var result = sut.IsSatisfiedBy(item);

			// Assert
			result.Should().BeTrue();
		}

		[Theory, AllInnerSpecificationsFail]
		public void ItShouldReturnFalseIfNoInnerSpecificationsAreSatisfied(IFixture fixture, TestType item)
		{
			// Arrange
			var sut = fixture.Create<OrSpecification<TestType>>();

			// Act
			var result = sut.IsSatisfiedBy(item);

			// Assert
			result.Should().BeFalse();
		}

		[Theory, SpecificationsAreMixed]
		public void ItShouldReturnTrueIfSomeInnerSpecificationsAreSatisfied(IFixture fixture, TestType item)
		{
			// Arrange
			var sut = fixture.Create<OrSpecification<TestType>>();

			// Act
			var result = sut.IsSatisfiedBy(item);

			// Assert
			result.Should().BeTrue();
		}
	}
}