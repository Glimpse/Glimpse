using System;
using System.Web.Mvc;

namespace Glimpse.Net.Plumbing
{
    public class GlimpseFilterCallMetadata
    {
        public GlimpseFilterCallMetadata(){}

        public GlimpseFilterCallMetadata(string category, Guid guid, string method, Filter innerFilter)
        {
            Category = category;
            Guid = guid;
            InnerFilter = innerFilter;
            Method = method;
            Order = innerFilter.Order;
            Scope = innerFilter.Scope;
            Type = innerFilter.Instance.GetType();
        }

        public string Category { get; set; }
        public Guid Guid { get; set; }
        public Filter InnerFilter { get; set; }
        public string Method { get; set; }
        public int? Order { get; set; }
        public FilterScope? Scope { get; set; }
        public Type Type { get; set; }

        public static GlimpseFilterCallMetadata ControllerAction(ActionDescriptor actionDescriptor)
        {
            return new GlimpseFilterCallMetadata
                       {
                           Category = "",
                           Guid = Guid.NewGuid(),
                           InnerFilter = null,
                           Order = null,
                           Method = actionDescriptor.GetName(),
                           Scope = null,
                           Type = actionDescriptor.ControllerDescriptor.ControllerType,
                       };
        }

        public static GlimpseFilterCallMetadata ActionResult(ActionResult actionResult)
        {
            return new GlimpseFilterCallMetadata
            {
                Category = "",
                Guid = Guid.NewGuid(),
                InnerFilter = null,
                Method = "ExecuteResult(ControllerContext context)",
                Order = null,
                Scope = null,
                Type = actionResult.GetType(),
            };
        }
    }
}