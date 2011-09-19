namespace Glimpse.Core2.Extensibility
{
    public interface IGlimpsePlugin<T>
    {
        object GetData(IServiceLocator<T> locator);
        string Name { get; }
    }
}
