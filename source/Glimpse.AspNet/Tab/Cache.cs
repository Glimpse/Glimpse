using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Caching;
using Glimpse.AspNet.Extensibility;
using Glimpse.AspNet.Model;
using Glimpse.AspNet.Support;
using Glimpse.Core.Extensibility;

namespace Glimpse.AspNet.Tab
{
    public class Cache : AspNetTab, ILayoutControl
    {
        private const string TestCacheKey = "testKey";
        private static readonly MethodInfo MethodInfoCacheGet = HttpRuntime.Cache.GetType().GetMethod("Get", BindingFlags.Instance | BindingFlags.NonPublic);
        private static Type TypeCacheEntity;
        private static PropertyInfo ProcessInfoUtcCreated;
        private static PropertyInfo ProcessInfoUtcExpires;
        private static PropertyInfo ProcessInfoSlidingExpiration;

        static Cache()
        {
            //need an item in the cache to call the MethodInfoCacheGet.Invoke below.
            HttpRuntime.Cache.Add(TestCacheKey, "", null, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration,
                CacheItemPriority.AboveNormal, null);

            object cacheEntry = MethodInfoCacheGet.Invoke(HttpRuntime.Cache, new object[] { TestCacheKey, 1 });
            HttpRuntime.Cache.Remove(TestCacheKey);

            TypeCacheEntity = cacheEntry.GetType();
            ProcessInfoUtcCreated = TypeCacheEntity.GetProperty("UtcCreated", BindingFlags.NonPublic | BindingFlags.Instance);
            ProcessInfoUtcExpires = TypeCacheEntity.GetProperty("UtcExpires", BindingFlags.NonPublic | BindingFlags.Instance);
            ProcessInfoSlidingExpiration = TypeCacheEntity.GetProperty("SlidingExpiration", BindingFlags.NonPublic | BindingFlags.Instance);
        }

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
            cacheModel.Configuration.EffectivePercentagePhysicalMemoryLimit = HttpRuntime.Cache.EffectivePercentagePhysicalMemoryLimit;
            cacheModel.Configuration.EffectivePrivateBytesLimit = HttpRuntime.Cache.EffectivePrivateBytesLimit;

            var list = HttpRuntime.Cache.Cast<DictionaryEntry>().ToList();
            foreach (var item in list)
            {
                try
                {
                    var cacheEntry = MethodInfoCacheGet.Invoke(HttpRuntime.Cache, new object[] { item.Key, 1 });

                    var cacheItemModel = new CacheItemModel();
                    cacheItemModel.Key = item.Key.ToString();
                    cacheItemModel.Value = Serialization.GetValueSafe(item.Value);
                    cacheItemModel.CreatedOn = GetCacheProperty(ProcessInfoUtcCreated, cacheEntry) as DateTime?;
                    cacheItemModel.ExpiresOn = GetCacheProperty(ProcessInfoUtcExpires, cacheEntry) as DateTime?;
                    cacheItemModel.SlidingExpiration = GetCacheProperty(ProcessInfoSlidingExpiration, cacheEntry) as TimeSpan?;

                    cacheModel.CacheItems.Add(cacheItemModel);
                }
                catch (Exception)
                {
                    return false;
                }
            } 

            return cacheModel;
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
