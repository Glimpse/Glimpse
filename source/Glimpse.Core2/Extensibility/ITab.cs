namespace Glimpse.Core2.Extensibility
{
    public interface ITab
    {
        object GetData(ITabContext context);
        string Name { get; }
    }
}
