using Common.Specifications;
using Common.UnitTests.TestingHelpers;
using FakeItEasy;
using Ploeh.AutoFixture;
using System.Collections.Generic;

namespace Common.UnitTests.Specifications
{
	internal abstract class BaseSpecificationsPassCustomization : ICustomization
	{
		protected abstract bool ResultForFirstSpecification { get; }
		protected abstract bool ResultForSecondSpecification { get; }
		protected abstract bool ResultForThirdSpecification { get; }

		public void Customize(IFixture fixture)
		{
			var item = fixture.Freeze<TestType>();

			var first = fixture.Create<ISpecification<TestType>>();
			var second = fixture.Create<ISpecification<TestType>>();
			var third = fixture.Create<ISpecification<TestType>>();

			A.CallTo(() => first.IsSatisfiedBy(item)).Returns(ResultForFirstSpecification);
			A.CallTo(() => second.IsSatisfiedBy(item)).Returns(ResultForSecondSpecification);
			A.CallTo(() => third.IsSatisfiedBy(item)).Returns(ResultForThirdSpecification);

			var array = new[] { first, second, third };

			fixture.Register<IEnumerable<ISpecification<TestType>>>(() => array);
			fixture.Register<ISpecification<TestType>[]>(() => array);
		}
	}

	internal class AllInnerSpecificationsPassCustomization : BaseSpecificationsPassCustomization
	{
		protected override bool ResultForFirstSpecification { get { return true; } }

		protected override bool ResultForSecondSpecification { get { return true; } }

		protected override bool ResultForThirdSpecification { get { return true; } }
	}

	public class AllInnerSpecificationsPassAttribute : AutoFakeItEasyDataAttribute
	{
		public AllInnerSpecificationsPassAttribute()
			: base(new AllInnerSpecificationsPassCustomization())
		{
		}
	}

	internal class AllInnerSpecificationsFailCustomization : BaseSpecificationsPassCustomization
	{
		protected override bool ResultForFirstSpecification { get { return false; } }

		protected override bool ResultForSecondSpecification { get { return false; } }

		protected override bool ResultForThirdSpecification { get { return false; } }
	}

	public class AllInnerSpecificationsFailAttribute : AutoFakeItEasyDataAttribute
	{
		public AllInnerSpecificationsFailAttribute()
			: base(new AllInnerSpecificationsFailCustomization())
		{
		}
	}

	internal class SpecificationsAreMixedCustomization : BaseSpecificationsPassCustomization
	{
		protected override bool ResultForFirstSpecification { get { return false; } }

		protected override bool ResultForSecondSpecification { get { return true; } }

		protected override bool ResultForThirdSpecification { get { return false; } }
	}

	public class SpecificationsAreMixedAttribute : AutoFakeItEasyDataAttribute
	{
		public SpecificationsAreMixedAttribute()
			: base(new SpecificationsAreMixedCustomization())
		{
		}
	}
}