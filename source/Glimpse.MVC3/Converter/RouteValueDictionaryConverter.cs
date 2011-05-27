using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Glimpse.Core.Extensibility;

namespace Glimpse.Mvc3.Converter
{
    [GlimpseConverter]
    internal class RouteValueDictionaryConverter : IGlimpseConverter
    {
        public IDictionary<string, object> Serialize(object obj)
        {
            var source = obj as RouteValueDictionary;
            if (source == null) return null;

            return source.ToDictionary(item => item.Key, item => item.Value == UrlParameter.Optional ? "_Optional_" : item.Value);
        }

        public IEnumerable<Type> SupportedTypes
        {
            get
            {
                yield return typeof(RouteValueDictionary);
                yield break;
            }
        }
    }
}
