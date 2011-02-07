using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Glimpse.Net.Converter
{
    public class HandleErrorAttributeConverter : JavaScriptConverter
    {
        public override object Deserialize(IDictionary<string, object> dictionary, Type type,
                                           JavaScriptSerializer serializer)
        {
            throw new NotImplementedException();
        }

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