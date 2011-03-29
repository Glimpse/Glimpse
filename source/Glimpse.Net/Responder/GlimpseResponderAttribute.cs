using System;
using System.ComponentModel.Composition;

namespace Glimpse.Net.Responder
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GlimpseResponderAttribute:ExportAttribute
    {
        public GlimpseResponderAttribute() : base(typeof(GlimpseResponder)) { }
    }
}
