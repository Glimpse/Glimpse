using Glimpse.Core.Extensibility;
using Glimpse.EF.Inspector.Core;

namespace Glimpse.EF.Inspector
{
    public class EntityFrameworkInspector : IInspector
    {
        public void Setup(IInspectorContext context)
        {
            EntityFrameworkExecutionBlock.Instance.Execute();
        }
    }
}