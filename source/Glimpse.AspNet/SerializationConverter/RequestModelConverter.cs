using System.Collections.Generic;
using Glimpse.AspNet.Tab;
using Glimpse.Core2.Extensibility;

namespace Glimpse.AspNet.SerializationConverter
{
    public class RequestModelConverter:SerializationConverter<RequestModel>
    {
        public override IDictionary<string, object> Convert(RequestModel request)
        {
            return new Dictionary<string, object>
                       {
                           {"cookie", request.Cookies}
                       };

        }
    }

    public class CookieCollectionConverter : SerializationConverter<ICollection<Cookie>>
    {
        public override IDictionary<string, object> Convert(ICollection<Cookie> cookies)
        {
            return new Dictionary<string, object>
                       {
                           {"any", "thing"}
                       };
        }
    }
}