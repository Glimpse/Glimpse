using System;
using System.Web.Mvc;
using Glimpse.Net.Extentions;

namespace Glimpse.Net.Plumbing
{
    public class GlimpseFilterCallMetadata
    {
        public GlimpseFilterCallMetadata(){}

        public GlimpseFilterCallMetadata(string category, Guid guid, string method, Filter innerFilter, bool isChild)
        {
            Category = category;
            Guid = guid;
            InnerFilter = innerFilter;
            Method = method;
            Order = innerFilter.Order;
            Scope = innerFilter.Scope;
            Type = innerFilter.Instance.GetType();
            IsChild = isChild;
        }

        public string Category { get; set; }
        public Guid Guid { get; set; }
        public Filter InnerFilter { get; set; }
        public bool IsChild { get; set; }
        public string Method { get; set; }
        public int? Order { get; set; }
        public FilterScope? Scope { get; set; }
        public Type Type { get; set; }

        public static GlimpseFilterCallMetadata ControllerAction(ActionDescriptor actionDescriptor, bool isChild)
        {
            return new GlimpseFilterCallMetadata
                       {
                           Category = "",
                           Guid = Guid.NewGuid(),
                           InnerFilter = null,
                           IsChild = isChild,
                           Order = null,
                           Method = actionDescriptor.GetMethodNameWithParameters(),
                           Scope = null,
                           Type = actionDescriptor.ControllerDescriptor.ControllerType,
                       };
        }

        public static GlimpseFilterCallMetadata ActionResult(ActionResult actionResult, bool isChild)
        {
            return new GlimpseFilterCallMetadata
            {
                Category = "",
                Guid = Guid.NewGuid(),
                InnerFilter = null,
                IsChild = isChild,
                Method = "ExecuteResult(ControllerContext context)",
                Order = null,
                Scope = null,
                Type = actionResult.GetType(),
            };
        }
    }
}