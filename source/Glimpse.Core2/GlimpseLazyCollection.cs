using System;
using System.Collections;
using System.Collections.Generic;

namespace Glimpse.Core2
{
    public class GlimpseLazyCollection<TPart, TPartMetadata>:IEnumerable<Lazy<TPart, TPartMetadata>> 
    {
        private IList<Lazy<TPart, TPartMetadata>> UserCollection { get; set; }
        private IList<Lazy<TPart, TPartMetadata>> MefCollection { get; set; }
        public DiscoverabilityPolicy Discoverability { get; set; }

        public GlimpseLazyCollection()
        {
            UserCollection = new List<Lazy<TPart, TPartMetadata>>();
            MefCollection = new List<Lazy<TPart, TPartMetadata>>();
            Discoverability = new DiscoverabilityPolicy<TPart, TPartMetadata>(MefCollection);
        }

        public IEnumerator<Lazy<TPart, TPartMetadata>> GetEnumerator()
        {
            foreach (var item in UserCollection)
                yield return item;

            foreach (var item in MefCollection)
                yield return item;

            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(Lazy<TPart, TPartMetadata> item)
        {
            UserCollection.Add(item);
        }

        public void Clear()
        {
            UserCollection.Clear();
            MefCollection.Clear();
        }

        public bool Contains(Lazy<TPart, TPartMetadata> item)
        {
            return UserCollection.Contains(item) || MefCollection.Contains(item);
        }

        public bool Remove(Lazy<TPart, TPartMetadata> item)
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
