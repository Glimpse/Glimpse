using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Metadata
{
    public class LayoutTabExtensions : ITabMetadataExtensions
    {
        public string Key
        {
            get { return "layout"; }
        }

        public object ProcessTab(ITab tab)
        {
            var layoutTab = tab as ITabLayout;
            return layoutTab != null ? layoutTab.GetLayout() : null;
        }
    }
}