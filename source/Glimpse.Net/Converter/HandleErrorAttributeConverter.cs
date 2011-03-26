using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Glimpse.Protocol;

namespace Glimpse.Net.Converter
{
    [GlimpseConverter]
    public class HandleErrorAttributeConverter : IGlimpseConverter
    {
        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            var source = obj as HandleErrorAttribute;
            if (source == null) return null;

            return new Dictionary<string, object>
                             {
                                 {"ExceptionType", source.ExceptionType.ToString()},
                                 {"Master", source.Master},
                                 {"View", source.View}
                             };
        }

        public override IEnumerable<Type> SupportedTypes
        {
            get
            {
                yield return typeof(HandleErrorAttribute);
                yield break;
            }
        }
    }
}