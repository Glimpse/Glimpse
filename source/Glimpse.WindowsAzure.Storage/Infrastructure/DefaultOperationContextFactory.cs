using Microsoft.WindowsAzure.Storage;

namespace Glimpse.WindowsAzure.Storage.Infrastructure
{
    public class DefaultOperationContextFactory
        : IOperationContextFactory
    {
        public OperationContext Create()
        {
            return new OperationContext();
        }
    }
}