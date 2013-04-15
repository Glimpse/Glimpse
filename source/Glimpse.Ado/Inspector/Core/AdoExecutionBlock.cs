using Glimpse.Core.Framework.Support;

namespace Glimpse.Ado.Inspector.Core
{
    internal class AdoExecutionBlock : ExecutionBlockBase
    {
        public static readonly AdoExecutionBlock Instance = new AdoExecutionBlock();

        private AdoExecutionBlock()
        {
            RegisterProvider(new DbProviderFactoriesExecutionTask(Logger)); 
        }
    }
}