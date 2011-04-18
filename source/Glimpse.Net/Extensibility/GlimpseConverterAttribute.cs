using System;
using System.ComponentModel.Composition;

namespace Glimpse.Net.Extensibility
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GlimpseConverterAttribute:ExportAttribute
    {
        public GlimpseConverterAttribute() : base(typeof(IGlimpseConverter)) { }
    }
}
