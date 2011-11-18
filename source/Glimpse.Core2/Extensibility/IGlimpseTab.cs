namespace Glimpse.Core2.Extensibility
{
    public interface IGlimpseTab
    {
        object GetData(IServiceLocator locator);
        string Name { get; }
    }
}
