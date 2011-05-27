using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Serialization;

namespace Glimpse.Core.Plumbing
{
    public class GlimpseContractResolver : DefaultContractResolver
    {
        protected override IValueProvider CreateMemberValueProvider(MemberInfo member)
        {
            var result = base.CreateMemberValueProvider(member);

            return new GlimpseValueProvider(result);
        }

        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            var baseMembers = base.GetSerializableMembers(objectType);

            //TODO: Make this provider or config based
            baseMembers = (from m in baseMembers where !m.Name.Equals("_entityWrapper") select m).ToList();
            return baseMembers;
        }
    }

    internal class GlimpseValueProvider : IValueProvider
    {
        private IValueProvider ValueProvider { get; set; }

        public GlimpseValueProvider(IValueProvider valueProvider)
        {
            ValueProvider = valueProvider;
        }

        public void SetValue(object target, object value)
        {
            ValueProvider.SetValue(target, value);
        }

        public object GetValue(object target)
        {
            try
            {
                return ValueProvider.GetValue(target);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}