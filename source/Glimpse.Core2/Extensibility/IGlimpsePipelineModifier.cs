namespace Glimpse.Core2.Extensibility
{
    public interface IGlimpsePipelineModifier
    {
        void Setup();
        void Teardown();
    }
}
