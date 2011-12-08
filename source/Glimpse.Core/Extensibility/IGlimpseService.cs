namespace Glimpse.Core.Extensibility
{
    public interface IGlimpseService
    {
        string Name { get; }
        void SetupInit();
    }
}