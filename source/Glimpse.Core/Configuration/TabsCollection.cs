using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    public class TabsCollection : DiscoverableCollection<ITab>
    {
        public TabsCollection(
            CollectionConfiguration configuration, 
            ILogger logger) : base(configuration, logger)
        {
        }
    }
}
