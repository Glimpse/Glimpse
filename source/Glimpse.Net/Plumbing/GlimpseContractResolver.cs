using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Serialization;

namespace Glimpse.WebForms.Plumbing
{
    public class GlimpseContractResolver : DefaultContractResolver
    {
        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            var baseMembers = base.GetSerializableMembers(objectType);

            //TODO: Make this provider or config based
            baseMembers = (from m in baseMembers where !m.Name.Equals("_entityWrapper") select m).ToList();
            return baseMembers;
        }
    }
}