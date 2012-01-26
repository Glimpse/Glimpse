using Glimpse.Core2.Framework;

namespace Glimpse.Core2.Extensibility
{
    public interface IResourceResultContext : IContext
    {
        IFrameworkProvider FrameworkProvider { get; }
        ISerializer Serializer { get; }
    }
}