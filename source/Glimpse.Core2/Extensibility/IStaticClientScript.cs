namespace Glimpse.Core2.Extensibility
{
    public interface IStaticClientScript:IClientScript
    {
        string GetUri(string version);
    }
}