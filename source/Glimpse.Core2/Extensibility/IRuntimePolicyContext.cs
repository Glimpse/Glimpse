using Glimpse.Core2.Framework;

namespace Glimpse.Core2.Extensibility
{
    public interface IRuntimePolicyContext
    {
        IRequestMetadata RequestMetadata { get; }
        ILogger Logger { get; }
        T GetRequestContext<T>() where T : class;
    }
}