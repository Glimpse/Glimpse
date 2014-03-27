using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    public class RuntimePoliciesCollection : DiscoverableCollection<IRuntimePolicy>
    {
        public RuntimePoliciesCollection(
            CollectionConfiguration configuration, 
            ILogger logger) : base(configuration, logger)
        {
        }
    }
}
