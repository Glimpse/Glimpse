using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using Glimpse.Protocol;

namespace Glimpse.Net.Converter
{
    [GlimpseConverter]
    internal class RouteValueDictionaryConverter : IGlimpseConverter
    {
        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            var source = obj as RouteValueDictionary;
            if (source == null) return null;

            return source.ToDictionary(item => item.Key, item => item.Value == UrlParameter.Optional ? "_Optional_" : item.Value);
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
