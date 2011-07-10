using System.Collections.Generic;

namespace Glimpse.Core.Extensibility
{
    public interface IProvideGlimpseStructuredLayout
    {
        GlimpseStructuredLayout StructuredLayout { get; }
    }

    public class GlimpseStructuredLayoutCell
    {
        public object Data { get; set; }

        public bool? Key { get; set; }

        public string Align { get; set; }

        public string Width { get; set; }

        public string Prefix { get; set; }

        public string Postfix { get; set; }

        public string ClassName { get; set; }

        public bool? IsCode { get; set; }

        public string CodeType { get; set; }
    }

    public class GlimpseStructuredLayout : List<GlimpseStructuredLayoutSection>
    {
    }

    public class GlimpseStructuredLayoutSection : List<GlimpseStructuredLayoutCell>
    {
    }
}