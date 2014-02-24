using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Metadata
{
    public class DocumentationTabExtensions : ITabMetadataExtensions
    {
        public string Key
        {
            get { return "documentationUri"; }
        }

        public object ProcessTab(ITab tab)
        {
            var documentationTab = tab as IDocumentation;
            return documentationTab != null ? documentationTab.DocumentationUri : null;
        }
    }
}
