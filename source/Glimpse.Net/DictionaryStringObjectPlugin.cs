using System;
using System.Collections.Generic;
using System.Web;
using Glimpse.Protocol;

namespace Glimpse.Net
{
    public abstract class DictionaryStringObjectPlugin : IGlimpsePlugin
    {
        protected IDictionary<string, string> Process(IDictionary<string, object> dictionary,
                                                      HttpApplication application)
        {
            var result = new Dictionary<string, string>();

            if (dictionary == null) return null;

            foreach (var key in dictionary.Keys)
            {
                result.Add(key, dictionary[key].ToString());
            }

            if (result.Count == 0) return null;

            return result;
        }

        public abstract object GetData(HttpApplication application);
        public abstract void SetupInit();
        public abstract string Name { get; }
    }
}