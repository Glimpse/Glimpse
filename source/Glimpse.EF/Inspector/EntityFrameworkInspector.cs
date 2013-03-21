using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Config;
using System.Reflection;
using Glimpse.Core.Extensibility; 
using Glimpse.EF.AlternateType;
using Glimpse.EF.Inspector.Core;

namespace Glimpse.EF.Inspector
{
    public class EntityFrameworkInspector : IInspector
    {
        public void Setup(IInspectorContext context)
        {
            var wrapper = new EntityFrameworkExecutionBlock();
            wrapper.Execute();
        }
    }
}