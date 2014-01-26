using Microsoft.WindowsAzure.Storage;

namespace Glimpse.WindowsAzure.Storage.Infrastructure
{
    public interface IOperationContextFactory
    {
        OperationContext Create();
    }
}