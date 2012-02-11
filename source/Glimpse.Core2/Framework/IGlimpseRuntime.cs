namespace Glimpse.Core2.Framework
{
    //TODO: Add contracts here?
    public interface IGlimpseRuntime
    {
        string Version { get; }
        void BeginRequest();
        void EndRequest();
        void ExecuteDefaultResource();
        void ExecuteResource(string resourceName, ResourceParameters parameters);
        void BeginSessionAccess();
        void EndSessionAccess();
        bool Initialize();
        bool IsInitialized { get; }
    }
}
