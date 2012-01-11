using Glimpse.Core2.Framework;

namespace Glimpse.Core2.Extensibility
{
    public interface IRuntimePolicyContext
    {
        IRequestMetadata RequestMetadata { get; }
        IGlimpseLogger Logger { get; }
        T GetRequestContext<T>() where T : class;
    }
}