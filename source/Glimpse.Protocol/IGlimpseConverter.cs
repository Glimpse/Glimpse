using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Glimpse.Protocol
{
    public abstract class IGlimpseConverter:JavaScriptConverter
    {
        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}