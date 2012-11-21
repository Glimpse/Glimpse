namespace Glimpse.Core.Extensibility
{
    // TODO: Provide a config only way to create static client scripts without implementing a class
    public interface IStaticClientScript : IClientScript
    {
        string GetUri(string version);
    }
}