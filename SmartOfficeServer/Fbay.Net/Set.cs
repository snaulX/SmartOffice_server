using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Fbay
{
    // thanks to https://shwanoff.ru/set/
    // original source code: https://github.com/shwanoff/Set
    public class Set<T> : IEnumerable<T>
    {
        private List<T> _items = new List<T>();

        public int Count => _items.Count;

        public void Add(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (!_items.Contains(item))
            {
                _items.Add(item);
            }
        }

        public void AddRange(IEnumerable<T> items)
        {
            foreach (T item in items)
                Add(item);
        }

        public void Remove(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (!_items.Contains(item))
            {
                throw new KeyNotFoundException($"Element {item} not found in Set.");
            }

            _items.Remove(item);
        }

        public bool Contains(T item) => _items.Contains(item);

        public static Set<T> Union(Set<T> set1, Set<T> set2)
        {
            if (set1 == null)
            {
                throw new ArgumentNullException(nameof(set1));
            }

            if (set2 == null)
            {
                throw new ArgumentNullException(nameof(set2));
            }

            var resultSet = new Set<T>();

            var items = new List<T>();

            if (set1._items != null && set1._items.Count > 0)
            {
                items.AddRange(new List<T>(set1._items));
            }

            if (set2._items != null && set2._items.Count > 0)
            {
                items.AddRange(new List<T>(set2._items));
            }

            resultSet._items = items.Distinct().ToList();

            return resultSet;
        }

        public static Set<T> Intersection(Set<T> set1, Set<T> set2)
        {
            if (set1 == null)
            {
                throw new ArgumentNullException(nameof(set1));
            }

            if (set2 == null)
            {
                throw new ArgumentNullException(nameof(set2));
            }

            var resultSet = new Set<T>();

            if (set1.Count < set2.Count)
            {
                foreach (var item in set1._items)
                {
                    if (set2._items.Contains(item))
                    {
                        resultSet.Add(item);
                    }
                }
            }
            else
            {
                foreach (var item in set2._items)
                {
                    if (set1._items.Contains(item))
                    {
                        resultSet.Add(item);
                    }
                }
            }

            return resultSet;
        }

        public static Set<T> Difference(Set<T> set1, Set<T> set2)
        {
            if (set1 == null)
            {
                throw new ArgumentNullException(nameof(set1));
            }

            if (set2 == null)
            {
                throw new ArgumentNullException(nameof(set2));
            }

            var resultSet = new Set<T>();

            foreach (var item in set1._items)
            {
                if (!set2._items.Contains(item))
                {
                    resultSet.Add(item);
                }
            }

            foreach (var item in set2._items)
            {
                if (!set1._items.Contains(item))
                {
                    resultSet.Add(item);
                }
            }

            resultSet._items = resultSet._items.Distinct().ToList();

            return resultSet;
        }

        public static bool Subset(Set<T> set1, Set<T> set2)
        {
            if (set1 == null)
            {
                throw new ArgumentNullException(nameof(set1));
            }

            if (set2 == null)
            {
                throw new ArgumentNullException(nameof(set2));
            }

            var result = set1._items.All(s => set2._items.Contains(s));
            return result;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }
}