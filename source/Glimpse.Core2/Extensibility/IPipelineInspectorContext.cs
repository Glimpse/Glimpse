namespace Glimpse.Core2.Extensibility
{
    public interface IPipelineInspectorContext:IContext
    {
        IProxyFactory ProxyFactory { get; }
    }
}