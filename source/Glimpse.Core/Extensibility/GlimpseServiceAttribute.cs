using System;
using System.ComponentModel.Composition;

namespace Glimpse.Core.Extensibility
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GlimpseServiceAttribute : ExportAttribute
    {
        public GlimpseServiceAttribute() : base(typeof(IGlimpseService)) { }
    }
}
