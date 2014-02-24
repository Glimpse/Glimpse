using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Metadata
{
    public class KeysHeadingsTabExtensions : ITabMetadataExtensions
    {
        public string Key
        {
            get { return "keysHeadings"; }
        }

        public object ProcessTab(ITab tab)
        {
            var layoutControlTab = tab as ILayoutControl;
            if (layoutControlTab != null)
            {
                return layoutControlTab.KeysHeadings;
            }

            return null;
        }
    }
}