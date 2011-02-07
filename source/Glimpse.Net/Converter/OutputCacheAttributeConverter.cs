using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI;

namespace Glimpse.Net.Converter
{
    public class OutputCacheAttributeConverter : JavaScriptConverter
    {
        public override object Deserialize(IDictionary<string, object> dictionary, Type type,
                                           JavaScriptSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            var source = obj as OutputCacheAttribute;
            if (source == null) return null;

            return new Dictionary<string, object>
                       {
                           {"CacheProfile", source.CacheProfile},
                           {"Duration", source.Duration},
                           {"Location", source.Location.ToString()},
                           {"NoStore", source.NoStore},
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