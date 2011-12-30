using System.Collections;
using System.Collections.Generic;

namespace Glimpse.Core2
{
    public class GlimpseCollection<T>:IEnumerable<T>
    {
        private IList<T> UserCollection { get; set; }
        private IList<T> MefCollection { get; set; }
        public DiscoverabilityPolicy Discoverability { get; set; }

        public GlimpseCollection()
        {
            UserCollection = new List<T>();
            MefCollection = new List<T>();
            Discoverability = new MefDiscoverabilityPolicy<T>(MefCollection);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in UserCollection)
                yield return item;

            foreach (var item in MefCollection)
                yield return item;

            yield break;
        }

        public void Add(T item)
        {
            UserCollection.Add(item);
        }

        public void Clear()
        {
            UserCollection.Clear();
            MefCollection.Clear();
        }

        public bool Contains(T item)
        {
            return UserCollection.Contains(item) || MefCollection.Contains(item);
        }

        public bool Remove(T item)
        {
            if (UserCollection.Remove(item))
                return true;

            return MefCollection.Remove(item);
        }

        public int Count
        {
            get { return UserCollection.Count + MefCollection.Count; }
        }
    }
}