namespace Glimpse.WindowsAzure.Infrastructure
{
    public interface IWindowsAzureEnvironment
    {
        bool IsAvailable { get; }
    }
}