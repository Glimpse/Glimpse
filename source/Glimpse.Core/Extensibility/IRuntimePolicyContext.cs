using Glimpse.Core.Framework;

namespace Glimpse.Core.Extensibility
{
    public interface IRuntimePolicyContext : IContext
    {
        IRequestMetadata RequestMetadata { get; }
        
        T GetRequestContext<T>() where T : class;
    }
}