using System;
using System.ComponentModel.Composition;

namespace Glimpse.WebForms.Extensibility
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GlimpseSanitizerAttribute : ExportAttribute
    {
        public GlimpseSanitizerAttribute() : base(typeof(IGlimpseSanitizer)) { }
    }
}