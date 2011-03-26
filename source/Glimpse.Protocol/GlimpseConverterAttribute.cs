using System;
using System.ComponentModel.Composition;

namespace Glimpse.Protocol
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GlimpseConverterAttribute:ExportAttribute
    {
        public GlimpseConverterAttribute() : base(typeof(IGlimpseConverter)) { }
    }
}
