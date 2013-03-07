using Glimpse.Ado.Inspector.Support;
using Glimpse.Core.Extensibility; 

namespace Glimpse.Ado.Inspector
{
    public class AdoInspector : IInspector
    {
        public void Setup(IInspectorContext context)
        {            
            var wrapDbProviderFactories = new WrapDbProviderFactories(context);
            wrapDbProviderFactories.Inject();              
        }
    }
}