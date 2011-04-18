using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Glimpse.Protocol;

namespace Glimpse.Net.Converter
{
    [GlimpseConverter]
    internal class OutputCacheAttributeConverter : IGlimpseConverter
    {
        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            var source = obj as OutputCacheAttribute;
            if (source == null) return null;

            return new Dictionary<string, object>
                       {
                           {"CacheProfile", source.CacheProfile},
                           {"Duration", source.Duration},
                           {"Location", source.Location.ToString()},
                           {"NoStore", source.NoStore.ToString()},
                           {"SqlDependency", source.SqlDependency},
                           {"VaryByContentEncoding", source.VaryByContentEncoding},
                           {"VaryByCustom", source.VaryByCustom},
                           {"VaryByHeader", source.VaryByHeader},
                           {"VaryByParam", source.VaryByParam},
                       };
        }

        public override IEnumerable<Type> SupportedTypes
        {
            get
            {
                yield return typeof (OutputCacheAttribute);
                yield break;
            }
        }
    }
}