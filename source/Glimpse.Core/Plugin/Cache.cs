using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Text;

namespace Glimpse.Core.Plugin
{
    [GlimpsePlugin]
    internal class Cache : IGlimpsePlugin, IProvideGlimpseHelp
    {
        public string Name
        {
            get { return "Cache"; }
        }

        public object GetData(HttpContextBase context)
        {
            var items = new List<object[]>{
                                  new[]{"Key", "Value", "Expiry", "SlidingExpiration"}
                              };

            var cacheEnumerator = HttpRuntime.Cache.GetEnumerator();
            while (cacheEnumerator.MoveNext())
            {
                var key = cacheEnumerator.Key.ToString();
                // if more properties want to be displayed in the future, here's a list of properties available
                //     Value - UtcCreated - State - UtcExpires - SlidingExpiration - ExpiresBucket - ExpiresEntryRef 
                //     UsageBucket - UsageEntryRef - UtcLastUsageUpdate - Dependency - Key - IsOutputCache - IsPublic
                items.Add(new []{ 
                    key, 
                    cacheEnumerator.Value,
                    GetCacheItemProperty(key, "UtcExpires"),
                    GetCacheItemProperty(key, "SlidingExpiration")
                });
            }

            return new
            {
                HttpRuntime.Cache.EffectivePercentagePhysicalMemoryLimit,
                HttpRuntime.Cache.EffectivePrivateBytesLimit,
                Domer = items
            };
        }

        public void SetupInit()
        {
        }

        public string HelpUrl
        {
            get { return "http://getGlimpse.com/Help/Plugin/Cache"; }
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