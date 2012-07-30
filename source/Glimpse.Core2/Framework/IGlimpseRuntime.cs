namespace Glimpse.Core2.Framework
{
    public interface IGlimpseRuntime
    {
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
