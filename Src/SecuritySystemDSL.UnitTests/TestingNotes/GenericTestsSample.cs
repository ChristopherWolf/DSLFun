//using System;
//using Xunit;
//using Xunit.Extensions;
//
//namespace SecuritySystemDSL.UnitTests
//{
//	public class Interval<T>
//	{
//		readonly IComparable<T> _first;
//		readonly IComparable<T> _second;
//
//		public Interval(IComparable<T> first, IComparable<T> second)
//		{
//			if (first == null) throw new ArgumentNullException("first");
//			if (second == null) throw new ArgumentNullException("second");
//
//			_first = first;
//			_second = second;
//		}
//
//		public IComparable<T> Minimum
//		{
//			get { return _first; }
//		}
//
//		public bool Contains(int value)
//		{
//			return false;
//		}
//	}
//
//	public abstract class IntervalFacts<T>
//	{
//		[Theory, AutoFakeItEasyData]
//		public void MinimumIsCorrect(IComparable<T> first, IComparable<T> second)
//		{
//			var sut = new Interval<T>(first, second);
//
//			IComparable<T> result = sut.Minimum;
//
//			Assert.Equal(result, first);
//		}
//	}
//
//	public class DecimalIntervalFacts : IntervalFacts<decimal> { }
//	public class StringIntervalFacts : IntervalFacts<string> { }
//	public class DateTimeIntervalFacts : IntervalFacts<DateTime> { }
//	public class TimSpanIntervalFacts : IntervalFacts<TimeSpan> { }
//
//	public class Int32IntervalFacts : IntervalFacts<int>
//	{
//		[Theory]
//		[InlineData(0, 0, 0, true)]
//		[InlineData(-1, 1, -1, true)]
//		[InlineData(-1, 1, 0, true)]
//		[InlineData(-1, 1, 1, true)]
//		[InlineData(-1, 1, -2, false)]
//		[InlineData(-1, 1, 2, false)]
//		public void Int32ContainsReturnsCorrectResult(
//			int minimum, int maximum, int value,
//			bool expectedResult)
//		{
//			var sut = new Interval<int>(minimum, maximum);
//
//			var result = sut.Contains(value);
//
//			Assert.Equal(expectedResult, result);
//		}
//
//		// More tests...
//	}
//}