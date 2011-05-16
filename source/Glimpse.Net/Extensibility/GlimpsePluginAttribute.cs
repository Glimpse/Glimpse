using System;
using System.ComponentModel.Composition;

namespace Glimpse.WebForms.Extensibility
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GlimpsePluginAttribute : ExportAttribute
    {
        public GlimpsePluginAttribute() : base(typeof(IGlimpsePlugin)){}
        public GlimpsePluginAttribute(bool sessionRequired, bool shouldSetupInInit = false) : base(typeof(IGlimpsePlugin))
        {
            SessionRequired = sessionRequired;
            ShouldSetupInInit = shouldSetupInInit;
        }
        public bool SessionRequired { get; set; }
        public bool ShouldSetupInInit { get; set; }
    }
}
