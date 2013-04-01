#if EF6Plus
using System.Data.Entity.Config; 
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework.Support;
using Glimpse.EF.AlternateType;

namespace Glimpse.EF.Inspector.Core
{
    public class DbConfigurationExecutionTask : IExecutionTask
    {
        public void Execute(ILogger logger)
        {
            DbConfiguration.SetConfiguration(new GlimpseDbConfiguration());
        }
    }
}
#endif