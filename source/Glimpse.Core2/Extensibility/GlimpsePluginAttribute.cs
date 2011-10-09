using System;
using System.ComponentModel.Composition;

namespace Glimpse.Core2.Extensibility
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GlimpsePluginAttribute:ExportAttribute, IGlimpsePluginMetadata
    {
        public GlimpsePluginAttribute():base(typeof(IGlimpsePlugin)){}

        public GlimpsePluginAttribute(Type requestContextType = null, bool sessionAccessRequired = false): base(typeof(IGlimpsePlugin))
        {
            RequestContextType = requestContextType;
            SessionAccessRequired = sessionAccessRequired;
        }

        public Type RequestContextType { get; set; }
        public bool SessionAccessRequired { get; set; }
    }
}
