namespace Glimpse.Core2.Extensibility
{
    public interface IPipelineInspector
    {
        void Setup(IPipelineInspectorContext context);
        void Teardown(IPipelineInspectorContext context);//TODO: Leverage IDisposable instead? Who will call teardown?
    }
}
