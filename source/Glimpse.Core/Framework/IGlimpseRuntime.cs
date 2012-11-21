namespace Glimpse.Core.Framework
{
    public interface IGlimpseRuntime
    {
        bool IsInitialized { get; }
        
        void BeginRequest();
        
        void EndRequest();
        
        void ExecuteDefaultResource();
        
        void ExecuteResource(string resourceName, ResourceParameters parameters);
        
        void BeginSessionAccess();
        
        void EndSessionAccess();
        
        bool Initialize();
    }
}
