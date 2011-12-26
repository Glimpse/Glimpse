using System;
using System.ComponentModel.Composition;

namespace Glimpse.Core2.Extensibility
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GlimpseValidatorAttribute:ExportAttribute
    {
         public GlimpseValidatorAttribute():base(typeof(IGlimpseValidator)){}
    }
}