using System;
using Common.Specifications;
using Common.UnitTests.TestingHelpers;
using FakeItEasy;
using Ploeh.AutoFixture;

namespace Common.UnitTests.Specifications
{
	public class SpecificationPair
	{
		readonly ISpecification<TestType> _lhs;
		readonly ISpecification<TestType> _rhs;

		public SpecificationPair(ISpecification<TestType> lhs, ISpecification<TestType> rhs)
		{
			if (lhs == null) throw new ArgumentNullException("lhs");
			if (rhs == null) throw new ArgumentNullException("rhs");

			_lhs = lhs;
			_rhs = rhs;
		}

		public ISpecification<TestType> Lhs { get { return _lhs; } }

		public ISpecification<TestType> Rhs { get { return _rhs; } }
	}

	internal abstract class BaseSpecificationPairCustomization : ICustomization
	{
		protected abstract bool ResultForLHSSpecification { get; }
		protected abstract bool ResultForRHSSpecification { get; }

		public void Customize(IFixture fixture)
		{
			var item = fixture.Freeze<TestType>();

			var lhs = fixture.Create<ISpecification<TestType>>();
			var rhs = fixture.Create<ISpecification<TestType>>();

			A.CallTo(() => lhs.IsSatisfiedBy(item)).Returns(ResultForLHSSpecification);
			A.CallTo(() => rhs.IsSatisfiedBy(item)).Returns(ResultForRHSSpecification);

			fixture.Inject(new SpecificationPair(lhs, rhs));
		}
	}

	internal class BothSpecificationsPassCustomization : BaseSpecificationPairCustomization
	{
		protected override bool ResultForLHSSpecification { get { return true; } }

		protected override bool ResultForRHSSpecification { get { return true; } }
	}

	public class BothSpecificationsPassAttribute : AutoFakeItEasyDataAttribute
	{
		public BothSpecificationsPassAttribute()
			: base(new BothSpecificationsPassCustomization())
		{
		}
	}

	internal class BothSpecificationsFailCustomization : BaseSpecificationPairCustomization
	{
		protected override bool ResultForLHSSpecification { get { return false; } }

		protected override bool ResultForRHSSpecification { get { return false; } }
	}

	public class BothSpecificationsFailAttribute : AutoFakeItEasyDataAttribute
	{
		public BothSpecificationsFailAttribute()
			: base(new BothSpecificationsFailCustomization())
		{
		}
	}

	internal class FirstSpecificationPassesAndSecondFailsCustomization : BaseSpecificationPairCustomization
	{
		protected override bool ResultForLHSSpecification { get { return false; } }

		protected override bool ResultForRHSSpecification { get { return true; } }
	}

	public class FirstSpecificationPassesAndSecondFailsAttribute : AutoFakeItEasyDataAttribute
	{
		public FirstSpecificationPassesAndSecondFailsAttribute()
			: base(new FirstSpecificationPassesAndSecondFailsCustomization())
		{
		}
	}

	internal class FirstSpecificationFailsAndSecondPassesCustomization : BaseSpecificationPairCustomization
	{
		protected override bool ResultForLHSSpecification { get { return true; } }

		protected override bool ResultForRHSSpecification { get { return false; } }
	}

	public class FirstSpecificationFailsAndSecondPassesAttribute : AutoFakeItEasyDataAttribute
	{
		public FirstSpecificationFailsAndSecondPassesAttribute()
			: base(new FirstSpecificationFailsAndSecondPassesCustomization())
		{
		}
	}
}