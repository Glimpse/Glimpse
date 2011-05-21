using System;
using System.ComponentModel.Composition;

namespace Glimpse.Core.Extensibility
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GlimpseHandlerAttribute : ExportAttribute
    {
        public GlimpseHandlerAttribute() : base(typeof(IGlimpseHandler)) { }
    }
}
