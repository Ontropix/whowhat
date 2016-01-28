using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WhoWhat.UI.WindowsPhone.Infrastructure
{

    public class OrderedObservableCollection<T> : ObservableCollection<T>
    {
        private readonly IComparer<T> comparer;
        private readonly bool allowDuplicates;
        private readonly bool reverseOrder;

        /// <summary>
        /// Initializes a new instance of the OrderedObservableCollection.
        /// </summary>
        public OrderedObservableCollection()
            : this(false, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the OrderedObservableCollection
        /// </summary>
        /// <param name="allowDuplicates">True if the collection should allow duplicate values</param>
        /// <param name="reverseOrder">True to reverse the order in which the items are sorted</param>
        public OrderedObservableCollection(bool allowDuplicates = false, bool reverseOrder = false)
        {
            comparer = new ComparableComparer<T>();
            this.allowDuplicates = allowDuplicates;
            this.reverseOrder = reverseOrder;
        }

        /// <summary>
        /// Initializes a new instance of the OrderedObservableCollection using an IComparer implementation.
        /// </summary>
        /// <param name="comparer">IComparer implementation used for the item comparisons during ordering</param>
        /// <param name="allowDuplicates">True if the collection should allow duplicate values</param>
        /// <param name="reverseOrder">True to reverse the order in which the items are sorted</param>
        public OrderedObservableCollection(IComparer<T> comparer, bool allowDuplicates = false, bool reverseOrder = false)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");
            this.comparer = comparer;
            this.allowDuplicates = allowDuplicates;
            this.reverseOrder = reverseOrder;
        }

        /// <summary>
        /// Initializes a new instance of the OrderedObservableCollection class using a Comparison delegate for
        /// the comparison logic.
        /// </summary>
        /// <param name="comparison">The comparison delegate used for the item comparisons during ordering</param>
        /// <param name="allowDuplicates">True if the collection should allow duplicate values</param>
        /// <param name="reverseOrder">True to reverse the order in which the items are sorted</param>
        public OrderedObservableCollection(Comparison<T> comparison, bool allowDuplicates = false, bool reverseOrder = false)
        {
            if (comparison == null)
                throw new ArgumentNullException("comparison");
            comparer = new ComparisonComparer<T>(comparison);
            this.allowDuplicates = allowDuplicates;
            this.reverseOrder = reverseOrder;
        }

        /// <summary>
        /// Specifies whether duplicate values are allowed in the collection
        /// </summary>
        public bool AllowDuplicates
        {
            get { return allowDuplicates; }
        }

        /// <summary>
        /// Specifies whether to sort the items of the collection in reverse order
        /// </summary>
        public bool ReverseOrder
        {
            get { return reverseOrder; }
        }

        protected override sealed void InsertItem(int index, T item)
        {
            int insertIndex = GetInsertIndex(item);
            if (insertIndex < 0) return;
            base.InsertItem(insertIndex, item);
        }

        protected override sealed void SetItem(int index, T item)
        {
            RemoveItem(index);
            int insertIndex = GetInsertIndex(item);
            if (insertIndex < 0) return;
            base.InsertItem(insertIndex, item);
        }

        protected sealed override void MoveItem(int oldIndex, int newIndex)
        {
            throw new InvalidOperationException("Cannot move items in an ordered collection");
        }

        /// <summary>
        /// Performs a comparison between two item of type T. By default, this uses the IComparer implementation
        /// or the Comparison delegate specified in the constructor, but derived types can override
        /// this method to specify their own custom logic.
        /// </summary>
        /// <param name="x">The first item to compare</param>
        /// <param name="y">The second item to compare</param>
        /// <returns>A signed integer - zero if the items are equal, less than zero if x is less than y and greater than zero if x is greater than y</returns>
        protected virtual int Compare(T x, T y)
        {
            return comparer.Compare(x, y);
        }

        private int ReverseComparisonIfNeeded(int comparison)
        {
            return reverseOrder ? -(comparison) : comparison;
        }

        private int GetInsertIndex(T item)
        {
            if (Count == 0)
                return 0;
            return Count <= SimpleAlgorithmThreshold ? GetInsertIndexSimple(item) : GetInsertIndexComplex(item);
        }

        //Performs a simple left-to-right search for the best location to insert the new item.
        //This algorithm is used while the collection size is small, i.e. less than or equal to the
        //value specified by the SimpleAlgorithmThreshold constant.
        private int GetInsertIndexSimple(T item)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                T existingItem = Items[i];
                int comparison = ReverseComparisonIfNeeded(Compare(existingItem, item));
                if (comparison == 0)
                    return allowDuplicates ? i : -1;
                if (comparison > 0)
                    return i;
            }
            return Count;
        }

        //Performs a divide-and-conquer search for the best location to insert the new item.
        //Since the list is already sorted, this is the fastest algorithm after the collection size
        //crosses a certain threshold.
        private int GetInsertIndexComplex(T item)
        {
            int minIndex = 0, maxIndex = Count - 1;
            while (minIndex <= maxIndex)
            {
                int pivotIndex = (maxIndex + minIndex) / 2;
                int comparison = ReverseComparisonIfNeeded(Compare(item, Items[pivotIndex]));
                if (comparison == 0)
                    return allowDuplicates ? pivotIndex : -1;
                if (comparison < 0)
                    maxIndex = pivotIndex - 1;
                else
                    minIndex = pivotIndex + 1;
            }
            return minIndex;
        }

        private const int SimpleAlgorithmThreshold = 10;


        //Comparer that uses the type's IComparable implementation to compare two values.
        private sealed class ComparableComparer<TItem> : IComparer<TItem>
        {
            int IComparer<TItem>.Compare(TItem x, TItem y)
            {
                return ((IComparable<TItem>)x).CompareTo(y);
            }
        }

        //Comparer that uses a Comparison delegate to perform the comparison logic.
        private sealed class ComparisonComparer<TItem> : IComparer<TItem>
        {
            private readonly Comparison<TItem> _comparison;

            internal ComparisonComparer(Comparison<TItem> comparison)
            {
                _comparison = comparison;
            }

            int IComparer<TItem>.Compare(TItem x, TItem y)
            {
                return _comparison(x, y);
            }
        }
    }
}
