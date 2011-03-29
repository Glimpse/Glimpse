using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using Glimpse.Protocol;

namespace Glimpse.Net
{
    public abstract class NameValueCollectionPlugin : IGlimpsePlugin
    {
        public abstract object GetData(HttpApplication application);
        public void SetupInit()
        {
            throw new NotImplementedException();
        }

        public abstract string Name { get; }

        protected IDictionary<string, string> Process(NameValueCollection collection, HttpApplication application)
        {
            var result = new Dictionary<string, string>();
            foreach (var key in collection.AllKeys)
            {
                var keyValue = string.IsNullOrEmpty(key) ? "*--*" : key;
                result.Add(keyValue, collection[keyValue]);
            }

            if (result.Count == 0) return null;

            return result;
        }
    }
}
