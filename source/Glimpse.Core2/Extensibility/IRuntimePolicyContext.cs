using Glimpse.Core2.Framework;

namespace Glimpse.Core2.Extensibility
{
    public interface IRuntimePolicyContext:IContext
    {
        IRequestMetadata RequestMetadata { get; }
        T GetRequestContext<T>() where T : class;
    }
}