using System.Collections.Generic;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.Framework
{
    public interface IGlimpseConfiguration
    {
        ICollection<IClientScript> ClientScripts { get; }
        IFrameworkProvider FrameworkProvider { get; }
        IHtmlEncoder HtmlEncoder { get; }
        ILogger Logger { get; }
        IPersistanceStore PersistanceStore { get; }
        ICollection<IPipelineInspector> PipelineInspectors { get; }
        ResourceEndpointConfiguration ResourceEndpoint { get; }
        ICollection<IResource> Resources { get; }
        ISerializer Serializer { get; }
        ICollection<ITab> Tabs { get; }
        ICollection<IRuntimePolicy> RuntimePolicies { get; }
        IResource DefaultResource { get; }
        RuntimePolicy DefaultRuntimePolicy { get; }
        IProxyFactory ProxyFactory { get; }
        IMessageBroker MessageBroker { get; }
        string EndpointBaseUri { get; }
    }
}