using System;
using System.ComponentModel.Composition;

namespace Glimpse.Protocol
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GlimpsePluginAttribute : ExportAttribute
    {
        public GlimpsePluginAttribute() : base(typeof(IGlimpsePlugin)){}
        public GlimpsePluginAttribute(bool sessionRequired) : base(typeof(IGlimpsePlugin))
        {
            SessionRequired = sessionRequired;
        }
        public bool SessionRequired { get; set; }
    }
}
