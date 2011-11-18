using System;
using System.ComponentModel.Composition;

namespace Glimpse.Core2.Extensibility
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GlimpseTabAttribute:ExportAttribute, IGlimpsePluginMetadata
    {
        public GlimpseTabAttribute():base(typeof(IGlimpseTab)){}

        public GlimpseTabAttribute(Type requestContextType = null): base(typeof(IGlimpseTab))
        {
            RequestContextType = requestContextType;
            LifeCycleSupport = LifeCycleSupport.EndRequest;
        }

        public Type RequestContextType { get; set; }
        public LifeCycleSupport LifeCycleSupport { get; set; }
    }
}
