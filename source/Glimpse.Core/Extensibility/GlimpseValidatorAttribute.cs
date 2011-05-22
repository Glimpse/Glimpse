using System;
using System.ComponentModel.Composition;
using Glimpse.Core.Plumbing;
using Glimpse.Core.Validator;

namespace Glimpse.Core.Extensibility
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GlimpseValidatorAttribute : ExportAttribute
    {
        public GlimpseValidatorAttribute() : base(typeof(IGlimpseValidator)) { }
    }
}
