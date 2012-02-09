using System.Collections.Generic;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.Framework
{
    public interface IGlimpseConfiguration
    {
        //TODO: Move contracts here
        ICollection<IClientScript> ClientScripts { get; set; }
        IFrameworkProvider FrameworkProvider { get; set; }
        IHtmlEncoder HtmlEncoder { get; set; }
        ILogger Logger { get; set; }
        IPersistanceStore PersistanceStore { get; set; }
        ICollection<IPipelineInspector> PipelineInspectors { get; set; }
        ResourceEndpointConfiguration ResourceEndpoint { get; set; }
        ICollection<IResource> Resources { get; set; }
        ISerializer Serializer { get; set; }
        ICollection<ITab> Tabs { get; set; }
        ICollection<IRuntimePolicy> RuntimePolicies { get; set; }
        IResource DefaultResource { get; set; }
        RuntimePolicy DefaultRuntimePolicy { get; set; }
    }
}