using System.Collections;
using System.Collections.Generic;

namespace Glimpse.Core2
{
    public class DiscoverableCollection<T>:IEnumerable<T>
    {
        private IList<T> ManualCollection { get; set; }
        private IList<T> DiscoveredCollection { get; set; }
        public DiscoverabilityPolicy Discoverability { get; set; }

        public DiscoverableCollection()
        {
            ManualCollection = new List<T>();
            DiscoveredCollection = new List<T>();
            Discoverability = new MefDiscoverabilityPolicy<T>(DiscoveredCollection);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in ManualCollection)
                yield return item;

            foreach (var item in DiscoveredCollection)
                yield return item;

            yield break;
        }

        public void Add(T item)
        {
            ManualCollection.Add(item);
        }

        public void Clear()
        {
            ManualCollection.Clear();
            DiscoveredCollection.Clear();
        }

        public bool Contains(T item)
        {
            return ManualCollection.Contains(item) || DiscoveredCollection.Contains(item);
        }

        public bool Remove(T item)
        {
            if (ManualCollection.Remove(item))
                return true;

            return DiscoveredCollection.Remove(item);
        }

        public int Count
        {
            get { return ManualCollection.Count + DiscoveredCollection.Count; }
        }
    }
}