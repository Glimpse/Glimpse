using System;
using System.ComponentModel.Composition;

namespace Glimpse.Core2.Extensibility
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GlimpsePluginAttribute:ExportAttribute, IGlimpsePluginMetadata
    {
        public GlimpsePluginAttribute():base(typeof(IGlimpsePlugin)){}

        public GlimpsePluginAttribute(Type requestContextType = null): base(typeof(IGlimpsePlugin))
        {
            RequestContextType = requestContextType;
            LifeCycleSupport = LifeCycleSupport.EndRequest;
        }

        public Type RequestContextType { get; set; }
        public LifeCycleSupport LifeCycleSupport { get; set; }
    }
}
