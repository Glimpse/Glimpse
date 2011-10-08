using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Glimpse.Core2
{
    public class PluginCollection:IList<object>
    {
        private IList<object> InnerList { get; set; }

        public PluginCollection(IList<object> innerList)
        {
            InnerList = innerList;
        }

        public IEnumerator<object> GetEnumerator()
        {
            return InnerList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(object item)
        {
            Contract.Requires(item != null);

            var itemType = item.GetType();
            var iface = itemType.GetInterface("IGlimpsePlugin`1");

            Contract.Assert(iface != null);
            
            InnerList.Add(item);
        }

        public void Clear()
        {
            InnerList.Clear();
        }

        public bool Contains(object item)
        {
            return InnerList.Contains(item);
        }

        public void CopyTo(object[] array, int arrayIndex)
        {
            InnerList.CopyTo(array, arrayIndex);
        }

        public bool Remove(object item)
        {
            return InnerList.Remove(item);
        }

        public int Count
        {
            get { return InnerList.Count; }
        }

        public bool IsReadOnly
        {
            get { return InnerList.IsReadOnly; }
        }

        public int IndexOf(object item)
        {
            return InnerList.IndexOf(item);
        }

        public void Insert(int index, object item)
        {
            InnerList.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            InnerList.RemoveAt(index);
        }

        public object this[int index]
        {
            get { return InnerList[index]; }
            set { InnerList[index] = value; }
        }
    }
}