using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Glimpse.WebForms.Extensibility;

namespace Glimpse.Mvc3.Converter
{
    [GlimpseConverter]
    internal class HandleErrorAttributeConverter : IGlimpseConverter
    {
        public IDictionary<string, object> Serialize(object obj)
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

        public IEnumerable<Type> SupportedTypes
        {
            get
            {
                yield return typeof(HandleErrorAttribute);
                yield break;
            }
        }
    }
}