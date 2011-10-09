using System;
using System.ComponentModel.Composition;

namespace Glimpse.Core2.Extensibility
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GlimpsePipelineModifierAttribute:ExportAttribute
    {
        public GlimpsePipelineModifierAttribute():base(typeof(IGlimpsePipelineModifier)){}
    }
}
