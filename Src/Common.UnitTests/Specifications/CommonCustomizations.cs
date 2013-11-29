using Common.Specifications;
using Common.UnitTests.TestingHelpers;
using FakeItEasy;
using Ploeh.AutoFixture;
using System.Collections.Generic;

namespace Common.UnitTests.Specifications
{
	internal class AllInnerSpecificationsPassCustomization : ICustomization
	{
		public void Customize(IFixture fixture)
		{
			var item = fixture.Freeze<TestType>();

			var first = fixture.Create<ISpecification<TestType>>();
			var second = fixture.Create<ISpecification<TestType>>();
			var third = fixture.Create<ISpecification<TestType>>();

			A.CallTo(() => first.IsSatisfiedBy(item)).Returns(true);
			A.CallTo(() => second.IsSatisfiedBy(item)).Returns(true);
			A.CallTo(() => third.IsSatisfiedBy(item)).Returns(true);

			var array = new[] {first, second, third};

			fixture.Register<IEnumerable<ISpecification<TestType>>>(() => array);
			fixture.Register<ISpecification<TestType>[]>(() => array);
		}
	}

	public class AllInnerSpecificationsPassAttribute : AutoFakeItEasyDataAttribute
	{
		public AllInnerSpecificationsPassAttribute()
			: base(new AllInnerSpecificationsPassCustomization())
		{
		}
	}

	internal class AllInnerSpecificationsFailCustomization : ICustomization
	{
		public void Customize(IFixture fixture)
		{
			var item = fixture.Freeze<TestType>();

			var first = fixture.Create<ISpecification<TestType>>();
			var second = fixture.Create<ISpecification<TestType>>();
			var third = fixture.Create<ISpecification<TestType>>();

			A.CallTo(() => first.IsSatisfiedBy(item)).Returns(false);
			A.CallTo(() => second.IsSatisfiedBy(item)).Returns(false);
			A.CallTo(() => third.IsSatisfiedBy(item)).Returns(false);

			var array = new[] { first, second, third };

			fixture.Register<IEnumerable<ISpecification<TestType>>>(() => array);
			fixture.Register<ISpecification<TestType>[]>(() => array);
		}
	}

	public class AllInnerSpecificationsFailAttribute : AutoFakeItEasyDataAttribute
	{
		public AllInnerSpecificationsFailAttribute()
			: base(new AllInnerSpecificationsFailCustomization())
		{
		}
	}

	internal class SpecificationsAreMixedCustomization : ICustomization
	{
		public void Customize(IFixture fixture)
		{
			var item = fixture.Freeze<TestType>();

			var first = fixture.Create<ISpecification<TestType>>();
			var second = fixture.Create<ISpecification<TestType>>();
			var third = fixture.Create<ISpecification<TestType>>();

			A.CallTo(() => first.IsSatisfiedBy(item)).Returns(false);
			A.CallTo(() => second.IsSatisfiedBy(item)).Returns(true);
			A.CallTo(() => third.IsSatisfiedBy(item)).Returns(false);

			var array = new[] { first, second, third };

			fixture.Register<IEnumerable<ISpecification<TestType>>>(() => array);
			fixture.Register<ISpecification<TestType>[]>(() => array);
		}
	}

	public class SpecificationsAreMixedAttribute : AutoFakeItEasyDataAttribute
	{
		public SpecificationsAreMixedAttribute()
			: base(new SpecificationsAreMixedCustomization())
		{
		}
	}
}