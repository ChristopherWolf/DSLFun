using System;
using System.Collections.Generic;

namespace Common
{
    public static class EnumerableExtensions
    {
		/// <summary>
		/// Returns each element in the input IEnumerable in a new IEnumerable sequence.
		/// 
		/// This method is useful if you want to cast something like a List 'TItem' as a read-only IEnumerable. 
		/// If you just cast the List to an IEnumerable and return it then the result can still be cast back to a List that can then be edited.
		/// 
		/// Repeating each input item as a new IEnumerable sequence prevents the original sequence from being edited.
		/// </summary>
		/// <typeparam name="TItem">The type of items in the IEnumerable.</typeparam>
		/// <param name="source">The source IEnumerable to get items from.</param>
		public static IEnumerable<TItem> Repeat<TItem>(this IEnumerable<TItem> source)
		{
			if (source == null) throw new ArgumentNullException("source");

			// ReSharper disable LoopCanBeConvertedToQuery
			foreach (var item in source)
			{
				yield return item;
			}
			// ReSharper restore LoopCanBeConvertedToQuery
		}
    }
}
