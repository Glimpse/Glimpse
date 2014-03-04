using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Metadata
{
    public class KeysHeadingsTabMetadata : ITabMetadata
    {
        public string Key
        {
            get { return "keysHeadings"; }
        }

        public object GetTabMetadata(ITab tab)
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