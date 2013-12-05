using Common.UnitTests.TestingHelpers;
using DSLExamples.RecurringEvents.SemanticModel;
using FakeItEasy;
using FluentAssertions;
using Ploeh.AutoFixture;
using Xunit.Extensions;
using Ploeh.SemanticComparison.Fluent;

// ReSharper disable CheckNamespace
namespace DSLExamples.UnitTests.RecurringEvents.SemanticModel.SpecificationExtensionsTests
// ReSharper restore CheckNamespace
{
	public class WhenUsingTheAndExtensionMethod
	{
		[Theory, AutoFakeItEasyData]
		public void ItShouldReturnTheExpectedCompositeSpecification(IFixture fixture)
		{
			// Arrange
			var lhsSpec = fixture.Create<ISpecification<int>>();
			A.CallTo(() => lhsSpec.ToString()).Returns("LHS");

			var rhsSpec = fixture.Create<ISpecification<int>>();
			A.CallTo(() => rhsSpec.ToString()).Returns("RHS");

			var likness = lhsSpec.AsSource()
								.OfLikeness<AndSpecification<int>>()
								.With(x => x.LHS).EqualsWhen((single, composite) => composite.LHS == lhsSpec)
								.With(x => x.RHS).EqualsWhen((single, composite) => composite.RHS == rhsSpec);

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
		public void ItShouldReturnTheExpectedCompositeSpecification(IFixture fixture)
		{
			// Arrange
			var lhsSpec = fixture.Create<ISpecification<int>>();
			A.CallTo(() => lhsSpec.ToString()).Returns("LHS");

			var rhsSpec = fixture.Create<ISpecification<int>>();
			A.CallTo(() => rhsSpec.ToString()).Returns("RHS");

			var likness = lhsSpec.AsSource()
								.OfLikeness<OrSpecification<int>>()
								.With(x => x.LHS).EqualsWhen((single, composite) => composite.LHS == lhsSpec)
								.With(x => x.RHS).EqualsWhen((single, composite) => composite.RHS == rhsSpec);

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