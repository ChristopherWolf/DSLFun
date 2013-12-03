﻿using System.Linq;
using Common.Specifications;
using Common.UnitTests.Specifications;
using Common.UnitTests.TestingHelpers;
using FakeItEasy;
using FluentAssertions;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Idioms;
using Xunit.Extensions;

// ReSharper disable CheckNamespace
namespace Common.UnitTests.EnumerableExtensionsTests.Specifications.AndSpecificationTests
// ReSharper restore CheckNamespace
{
	public class WhenVerifyingArchitecturalConstraints
	{
		[Theory, AutoFakeItEasyData]
		public void ItShouldBeASpecification(AndSpecification<TestType> sut)
		{
			sut.Should().BeAssignableTo<ISpecification<TestType>>();
		}

		[Theory, AutoFakeItEasyData]
		public void AllConstructorArgumentsShouldBeExposedAsWellBehavedReadOnlyProperties(IFixture fixture)
		{
			// Arrange
			var assertion = new ConstructorInitializedMemberAssertion(fixture);
			var type = typeof(AndSpecification<TestType>);

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
		[InlineData(true, false, false)]
		[InlineData(false, true, false)]
		[InlineData(false, false, false)]
		public void ItShouldReturnTheCorrectValueForAllPossibleCombinations(bool lhsResult, bool rhsResult, bool expected)
		{
			// Arrange
			var item = new TestType();

			var lhs = A.Fake<ISpecification<TestType>>();
			A.CallTo(() => lhs.IsSatisfiedBy(item)).Returns(lhsResult);

			var rhs = A.Fake<ISpecification<TestType>>();
			A.CallTo(() => rhs.IsSatisfiedBy(item)).Returns(rhsResult);

			var sut = new AndSpecification<TestType>(lhs, rhs);

			// Act
			var result = sut.IsSatisfiedBy(item);

			// Assert
			result.Should().Be(expected);
		}

//		[Theory, BothSpecificationsPass]
//		public void ItShouldReturnTrueIfBothSpecificationsAreSatisfied(TestType item, SpecificationPair pair)
//		{
//			// Arrange
//			var sut = new AndSpecification<TestType>(pair.Lhs, pair.Rhs);
//
//			// Act
//			var result = sut.IsSatisfiedBy(item);
//
//			// Assert
//			result.Should().BeTrue();
//		}
//
//		[Theory, BothSpecificationsFail]
//		public void ItShouldReturnFalseIfNeitherSpecificationIsSatisfied(TestType item, SpecificationPair pair)
//		{
//			// Arrange
//			var sut = new AndSpecification<TestType>(pair.Lhs, pair.Rhs);
//
//			// Act
//			var result = sut.IsSatisfiedBy(item);
//
//			// Assert
//			result.Should().BeFalse();
//		}
//
//		[Theory, FirstSpecificationFailsAndSecondPasses]
//		public void ItShouldReturnFalseIfFirstSpecificationFails(TestType item, SpecificationPair pair)
//		{
//			// Arrange
//			var sut = new AndSpecification<TestType>(pair.Lhs, pair.Rhs);
//
//			// Act
//			var result = sut.IsSatisfiedBy(item);
//
//			// Assert
//			result.Should().BeFalse();
//		}
//
//		[Theory, FirstSpecificationPassesAndSecondFails]
//		public void ItShouldReturnFalseIfSecondSpecificationFails(TestType item, SpecificationPair pair)
//		{
//			// Arrange
//			var sut = new AndSpecification<TestType>(pair.Lhs, pair.Rhs);
//
//			// Act
//			var result = sut.IsSatisfiedBy(item);
//
//			// Assert
//			result.Should().BeFalse();
//		}
	}
}