using System;
using System.ComponentModel.Composition;

namespace Glimpse.Core.Extensibility
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GlimpseSanitizerAttribute : ExportAttribute
    {
        public GlimpseSanitizerAttribute() : base(typeof(IGlimpseSanitizer)) { }
    }
}