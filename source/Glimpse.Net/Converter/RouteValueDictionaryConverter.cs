using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using Glimpse.Protocol;

namespace Glimpse.Net.Converter
{
    [GlimpseConverter]
    public class RouteValueDictionaryConverter : IGlimpseConverter
    {
        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            var source = obj as RouteValueDictionary;
            if (source == null) return null;

            var result = new Dictionary<string, object>();

            foreach (var item in source)
            {
                result.Add(item.Key, item.Value == UrlParameter.Optional ? "_Optional_" : item.Value);
            }

            return result;
        }

        public override IEnumerable<Type> SupportedTypes
        {
            get
            {
                yield return typeof(RouteValueDictionary);
                yield break;
            }
        }
    }
}
