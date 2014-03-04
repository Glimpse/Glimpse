using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Metadata
{
    public class DocumentationTabMetadata : ITabMetadata
    {
        public string Key
        {
            get { return "documentationUri"; }
        }

        public object GetTabMetadata(ITab tab)
        {
            var documentationTab = tab as IDocumentation;
            return documentationTab != null ? documentationTab.DocumentationUri : null;
        }
    }
}
