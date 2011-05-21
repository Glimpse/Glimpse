using System;
using System.ComponentModel.Composition;

namespace Glimpse.WebForms.Extensibility
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GlimpseHandlerAttribute : ExportAttribute
    {
        public GlimpseHandlerAttribute() : base(typeof(IGlimpseHandler)) { }
    }
}
