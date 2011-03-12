using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;

namespace Glimpse.Net.Converter
{
    public class RouteValueDictionaryConverter : JavaScriptConverter
    {
        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            throw new NotImplementedException();
        }

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
