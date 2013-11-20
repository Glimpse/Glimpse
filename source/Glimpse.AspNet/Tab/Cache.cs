using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Glimpse.AspNet.Extensibility;
using Glimpse.AspNet.Support;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Tab.Assist;

namespace Glimpse.AspNet.Tab
{
    public class Cache : AspNetTab, ILayoutControl
    {
        public override string Name
        {
            get { return "Cache"; }
        }

        public bool KeysHeadings
        {
            get { return true; }
        }
    
        public override object GetData(ITabContext context)
        {
            var items = new List<object>();

            var cacheEnumerator = HttpRuntime.Cache.GetEnumerator();
            while (cacheEnumerator.MoveNext())
            {
                var key = cacheEnumerator.Key.ToString();
                // if more properties want to be displayed in the future, here's a list of properties available
                //     Value - UtcCreated - State - UtcExpires - SlidingExpiration - ExpiresBucket - ExpiresEntryRef 
                //     UsageBucket - UsageEntryRef - UtcLastUsageUpdate - Dependency - Key - IsOutputCache - IsPublic
                items.Add(new { 
                    Key = key,
                    Value = Serialization.GetValueSafe(cacheEnumerator.Value),
                    Created = GetCacheItemProperty(key, "UtcCreated"),
                    Expiry = GetCacheItemProperty(key, "UtcExpires"),
                    SlidingExpiration = GetCacheItemProperty(key, "SlidingExpiration")
                });
            }

            return new
            {
                Settings = new {
                    PhysicalMemoryLimit = HttpRuntime.Cache.EffectivePercentagePhysicalMemoryLimit,
                    PrivateBytesLimit = HttpRuntime.Cache.EffectivePrivateBytesLimit,
                },
                Items = items
            };
        }
        
        private object GetCacheItemProperty(string cacheKey, string propertyName)
        {
            object cacheEntry = HttpRuntime.Cache.GetType().GetMethod("Get", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(HttpRuntime.Cache, new object[] { cacheKey, 1 });
            PropertyInfo property = cacheEntry.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance);
            var propertyValue = property.GetValue(cacheEntry, null);

            return propertyValue;
        }
    }
}
