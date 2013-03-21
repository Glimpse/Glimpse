using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework.Support
{
    public interface IExecutionTask
    {
        void Execute(ILogger logger);
    }
}