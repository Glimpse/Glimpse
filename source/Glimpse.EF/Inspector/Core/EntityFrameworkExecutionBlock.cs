using Glimpse.Core.Framework.Support;

namespace Glimpse.EF.Inspector.Core
{
    internal class EntityFrameworkExecutionBlock : ExecutionBlockBase
    {
        public static readonly EntityFrameworkExecutionBlock Instance = new EntityFrameworkExecutionBlock();

        private EntityFrameworkExecutionBlock()
        {
#if !EF1
            RegisterProvider(new DbConnectionFactoriesExecutionTask(Logger));
#endif

#if EF6Plus
            RegisterProvider(new DbConfigurationExecutionTask());
#endif
        }
    }
}
