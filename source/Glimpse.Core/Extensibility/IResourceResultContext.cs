using Glimpse.Core.Framework;

namespace Glimpse.Core.Extensibility
{
    public interface IResourceResultContext : IContext
    {
        IFrameworkProvider FrameworkProvider { get; }
        ISerializer Serializer { get; }
        IHtmlEncoder HtmlEncoder { get; }
    }
}