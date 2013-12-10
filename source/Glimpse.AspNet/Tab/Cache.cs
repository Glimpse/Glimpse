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
        private static readonly MethodInfo MethodInfoCacheGet = HttpRuntime.Cache.GetType().GetMethod("Get", BindingFlags.Instance | BindingFlags.NonPublic);
        private static Type TypeCacheEntity;
        private static PropertyInfo ProcessInfoUtcCreated;
        private static PropertyInfo ProcessInfoUtcExpires;
        private static PropertyInfo ProcessInfoSlidingExpiration;


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
                var currentCacheEntry = cacheEnumerator.Entry; 

                CacheItemModel cacheItemModel = null;
                if (TryGetCacheItemModel(currentCacheEntry, out cacheItemModel))
                {
                    cacheModel.CacheItems.Add(cacheItemModel);
                    cacheModel.Configuration.EffectivePercentagePhysicalMemoryLimit = HttpRuntime.Cache.EffectivePercentagePhysicalMemoryLimit;
                    cacheModel.Configuration.EffectivePrivateBytesLimit = HttpRuntime.Cache.EffectivePrivateBytesLimit;
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
                cacheEntry = MethodInfoCacheGet.Invoke(HttpRuntime.Cache, new object[] { currentCacheEntry.Key, 1 });
            }
            catch (NullReferenceException)
            {
                cacheItemModel = null;
                return false;
            }

            cacheItemModel.Key = currentCacheEntry.Key.ToString();
            cacheItemModel.Value = Serialization.GetValueSafe(currentCacheEntry.Value);

            if (TypeCacheEntity == null)
            {
                TypeCacheEntity = cacheEntry.GetType();
                ProcessInfoUtcCreated = TypeCacheEntity.GetProperty("UtcCreated", BindingFlags.NonPublic | BindingFlags.Instance);
                ProcessInfoUtcExpires = TypeCacheEntity.GetProperty("UtcExpires", BindingFlags.NonPublic | BindingFlags.Instance);
                ProcessInfoSlidingExpiration = TypeCacheEntity.GetProperty("SlidingExpiration", BindingFlags.NonPublic | BindingFlags.Instance);
            }

            cacheItemModel.CreatedOn = GetCacheProperty(ProcessInfoUtcCreated, cacheEntry);
            cacheItemModel.ExpiresOn = GetCacheProperty(ProcessInfoUtcExpires, cacheEntry);
            cacheItemModel.SlidingExpiration = GetCacheProperty(ProcessInfoSlidingExpiration, cacheEntry);

            return true;
        }

        private object GetCacheProperty(PropertyInfo property, object cacheEntry)
        { 
            try
            {
                return property.GetValue(cacheEntry, null);
            }
            catch (Exception)
            {
                return null;    
            }
        }
    }
}
