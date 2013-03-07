using Glimpse.Core.Extensibility;
using Glimpse.EF.Inspector.Support;

namespace Glimpse.EF.Inspector
{
    public class EntityFrameworkInspector : IInspector
    {
        public void Setup(IInspectorContext context)
        {             
            var wrapDbConnectionFactories = new WrapDbConnectionFactories();
            wrapDbConnectionFactories.Inject();

            var wrapCachedMetadata = new WrapCachedMetadata();
            wrapCachedMetadata.Inject();              
        }
    }
}