using System;
using System.ComponentModel.Composition;

namespace Glimpse.Core2.Extensibility
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GlimpseValidatorAttribute:ExportAttribute, IGlimpseValidatorMetadata
    {
         public GlimpseValidatorAttribute():base(typeof(IGlimpseValidator)){}
        
        public GlimpseValidatorAttribute(RuntimePhase? runtimePhase):base(typeof(IGlimpseValidator))
        {
            RuntimePhase = runtimePhase;
        }

         public RuntimePhase? RuntimePhase { get; set; }
    }
}