using System.Linq;
using Common.Specifications;
using Common.UnitTests.TestingHelpers;
using FluentAssertions;
using Ploeh.AutoFixture;
using Xunit.Extensions;

// ReSharper disable CheckNamespace
namespace Common.UnitTests.Specifications.Dates.IsBetweenSpecificationTests
// ReSharper restore CheckNamespace
{
	#region Customizations

	internal class StartValueIsBeforeEndCustomization : ICustomization
	{
		public void Customize(IFixture fixture)
		{
			var start = fixture.Create<int>();
			var generator = fixture.Create<Generator<int>>();

			// Ensure end is greater than start and that start and end are not directly adjacent
			var end = generator.First(x => x > start + 10);

			fixture.Inject(new StartAndEndTuple(start, end));
		}
	}

	public class StartValueIsBeforeEndAttribute : AutoFakeItEasyDataAttribute
	{
		public StartValueIsBeforeEndAttribute()
			: base(new StartValueIsBeforeEndCustomization())
		{
		}
	}

	public class StartAndEndTuple
	{
		readonly int _start;
		readonly int _end;

		public StartAndEndTuple(int start, int end)
		{
			_start = start;
			_end = end;
		}

		public int Start { get { return _start; } }

		public int End { get { return _end; } }
	}

	#endregion

	public class WhenVerifyingArchitecturalConstraints
	{
		[Theory, AutoFakeItEasyData]
		public void ItShouldBe(IFixture fixture, IsBetweenSpecification<int> sut)
		{
			sut.Should().BeAssignableTo<ISpecification<int>>();
		}

		public class WhenTestingIfSpecificationIsCorrect
		{
			[Theory, StartValueIsBeforeEnd]
			public void ItShouldReturnTrueIfItemIsEqualToStart(StartAndEndTuple tuple)
			{
				// Arrange
				var sut = new IsBetweenSpecification<int>(tuple.Start, tuple.End);

				// Act
				var result = sut.IsSatisfiedBy(tuple.Start);

				// Assert
				result.Should().BeTrue();
			}

			[Theory, StartValueIsBeforeEnd]
			public void ItShouldReturnTrueIfItemIsEqualToEnd(StartAndEndTuple tuple)
			{
				// Arrange
				var sut = new IsBetweenSpecification<int>(tuple.Start, tuple.End);

				// Act
				var result = sut.IsSatisfiedBy(tuple.End);

				// Assert
				result.Should().BeTrue();
			}

			[Theory, StartValueIsBeforeEnd]
			public void ItShouldReturnTrueIfItemIsBetweenStartAndEnd(StartAndEndTuple tuple, Generator<int> generator)
			{
				// Arrange
				var item = generator.First(x => x > tuple.Start && x < tuple.End);

				var sut = new IsBetweenSpecification<int>(tuple.Start, tuple.End);

				// Act
				var result = sut.IsSatisfiedBy(item);

				// Assert
				result.Should().BeTrue();
			}

			[Theory, StartValueIsBeforeEnd]
			public void ItShouldReturnFalseIfItemIsBeforeStart(StartAndEndTuple tuple, Generator<int> generator)
			{
				// Arrange
				var item = generator.First(x => x < tuple.Start);

				var sut = new IsBetweenSpecification<int>(tuple.Start, tuple.End);

				// Act
				var result = sut.IsSatisfiedBy(item);

				// Assert
				result.Should().BeFalse();
			}

			[Theory, StartValueIsBeforeEnd]
			public void ItShouldReturnFalseIfItemIsAfterEnd(StartAndEndTuple tuple, Generator<int> generator)
			{
				// Arrange
				var item = generator.First(x => x > tuple.End);

				var sut = new IsBetweenSpecification<int>(tuple.Start, tuple.End);

				// Act
				var result = sut.IsSatisfiedBy(item);

				// Assert
				result.Should().BeFalse();
			}
		}
	}
}