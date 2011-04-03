using System.Web.Mvc;

namespace Glimpse.Net.Plumbing
{
    public class GlimpseViewEngineCallMetadata
    {
        public ViewEngineResult ViewEngineResult { get; set; }
        public bool IsPartial { get; set; }
        public string ViewName { get; set; }
        public string MasterName { get; set; }
        public bool UseCache { get; set; }
        public GlimpseView GlimpseView { get; set; }
        public string ViewEngineName { get; set; }
    }
}
