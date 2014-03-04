using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Metadata
{
    public class LayoutTabMetadata : ITabMetadata
    {
        public string Key
        {
            get { return "layout"; }
        }

        public object GetTabMetadata(ITab tab)
        {
            var layoutTab = tab as ITabLayout;
            return layoutTab != null ? layoutTab.GetLayout() : null;
        }
    }
}