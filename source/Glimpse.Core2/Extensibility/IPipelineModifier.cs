namespace Glimpse.Core2.Extensibility
{
    public interface IPipelineModifier
    {
        void Setup();
        void Teardown();
    }
}
