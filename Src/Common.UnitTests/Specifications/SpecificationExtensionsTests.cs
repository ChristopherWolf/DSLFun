using Common.Specifications;
using Common.UnitTests.TestingHelpers;
using FakeItEasy;
using FluentAssertions;
using Ploeh.AutoFixture;
using Xunit.Extensions;
using Ploeh.SemanticComparison.Fluent;

// ReSharper disable CheckNamespace
namespace Common.UnitTests.Specifications.SpecificationExtensionsTests
// ReSharper restore CheckNamespace
{
	public class WhenUsingTheAndExtensionMethod
	{
		[Theory, AutoFakeItEasyData]
		public void ItShouldReturnTheExpectedValue(IFixture fixture)
		{
			// Arrange
			var lhsSpec = fixture.Create<ISpecification<int>>();
			A.CallTo(() => lhsSpec.ToString()).Returns("LHS");

			var rhsSpec = fixture.Create<ISpecification<int>>();
			A.CallTo(() => rhsSpec.ToString()).Returns("RHS");

			var likness = lhsSpec.AsSource()
								.OfLikeness<AndSpecification<int>>()
								.With(x => x.Lhs).EqualsWhen((specification, andSpecification) => andSpecification.Lhs == specification)
								.With(x => x.Rhs).EqualsWhen((specification, andSpecification) => andSpecification.Rhs == specification);

			// Act
			var result = lhsSpec.And(rhsSpec);

			// Assert
			result.Should().NotBeNull();
			result.Should().BeOfType<AndSpecification<int>>();
			likness.ShouldEqual(result.As<AndSpecification<int>>());
		}
	}

	public class WhenUsingTheOrExtensionMethod
	{
		[Theory, AutoFakeItEasyData]
		public void ItShouldReturnTheExpectedValue(IFixture fixture)
		{
			// Arrange
			var lhsSpec = fixture.Create<ISpecification<int>>();
			A.CallTo(() => lhsSpec.ToString()).Returns("LHS");

			var rhsSpec = fixture.Create<ISpecification<int>>();
			A.CallTo(() => rhsSpec.ToString()).Returns("RHS");

			var likness = lhsSpec.AsSource()
								.OfLikeness<OrSpecification<int>>()
								.With(x => x.Lhs).EqualsWhen((specification, andSpecification) => andSpecification.Lhs == specification)
								.With(x => x.Rhs).EqualsWhen((specification, andSpecification) => andSpecification.Rhs == specification);

			// Act
			var result = lhsSpec.Or(rhsSpec);

			// Assert
			result.Should().NotBeNull();
			result.Should().BeOfType<OrSpecification<int>>();
			likness.ShouldEqual(result.As<OrSpecification<int>>());
		}
	}

	public class WhenUsingTheNotExtensionMethod
	{
		[Theory, AutoFakeItEasyData]
		public void ItShouldReturnTheExpectedValue(IFixture fixture)
		{
			// Arrange
			var sourceSpec = fixture.Create<ISpecification<int>>();
			A.CallTo(() => sourceSpec.ToString()).Returns("sourceSpec");

			// Act
			var result = sourceSpec.Not();

			// Assert
			result.Should().NotBeNull();
			result.Should().BeOfType<NotSpecification<int>>();
			result.As<NotSpecification<int>>().InnerSpecification.Should().BeSameAs(sourceSpec);
		}
	}
}