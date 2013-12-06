using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Web;
using Glimpse.AspNet.Extensibility;
using Glimpse.AspNet.Model;
using Glimpse.AspNet.Support;
using Glimpse.Core.Extensibility;

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
            var cacheModel = new CacheModel();
            
            var cacheEnumerator = HttpRuntime.Cache.GetEnumerator();

            while (cacheEnumerator.MoveNext())
            {
                DictionaryEntry currentCacheEntry = cacheEnumerator.Entry;
                // if more properties want to be displayed in the future, here's a list of properties available
                //     Value - UtcCreated - State - UtcExpires - SlidingExpiration - ExpiresBucket - ExpiresEntryRef 
                //     UsageBucket - UsageEntryRef - UtcLastUsageUpdate - Dependency - Key - IsOutputCache - IsPublic

                CacheItemModel cacheItemModel = null;
                if (TryGetCacheItemModel(currentCacheEntry, out cacheItemModel))
                {
                    cacheModel.CacheItems.Add(cacheItemModel);
                    cacheModel.EffectivePercentagePhysicalMemoryLimit = HttpRuntime.Cache.EffectivePercentagePhysicalMemoryLimit;
                    cacheModel.EffectivePrivateBytesLimit = HttpRuntime.Cache.EffectivePrivateBytesLimit;
                }
            }

            return cacheModel;
        }

        private bool TryGetCacheItemModel(DictionaryEntry currentCacheEntry, out CacheItemModel cacheItemModel)
        {
            cacheItemModel = new CacheItemModel();
            object cacheEntry;
            try
            {
                cacheEntry =
                    HttpRuntime.Cache.GetType()
                               .GetMethod("Get", BindingFlags.Instance | BindingFlags.NonPublic)
                               .Invoke(HttpRuntime.Cache, new object[] {currentCacheEntry.Key, 1});
            }
            catch (NullReferenceException)
            {
                cacheItemModel = null;
                return false;
            }

            cacheItemModel.Key = currentCacheEntry.Key.ToString();
            cacheItemModel.Value = Serialization.GetValueSafe(currentCacheEntry.Value);

            cacheItemModel.CreatedOn = GetCacheProperty<DateTime>("UtcCreated", cacheEntry);
            cacheItemModel.ExpiresOn = GetCacheProperty<DateTime>("UtcExpires", cacheEntry);
            cacheItemModel.SlidingExpiration = GetCacheProperty<TimeSpan>("SlidingExpiration", cacheEntry);

            return true;
        }

        private T GetCacheProperty<T>(string propertyName, object cacheEntry)
        {
            PropertyInfo property = cacheEntry.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance);
            var converter = TypeDescriptor.GetConverter(typeof(T));
            try
            {
                return (T)converter.ConvertFromString(property.GetValue(cacheEntry, null).ToString());
            }
            catch (Exception)
            {
                return default(T);    
            }
        }
    }
}
