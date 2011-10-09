namespace Glimpse.Core2.Extensibility
{
    public interface IGlimpsePlugin
    {
        object GetData(IServiceLocator locator);
        string Name { get; }
    }
}
