using System.Collections.Generic;

namespace Glimpse.WindowsAzure.Storage.Infrastructure.Inspections
{
    public interface IWindowsAzureStorageInspector
    {
        IEnumerable<string> Inspect();

        IEnumerable<string> Inspect(WindowsAzureStorageTimelineMessage message);
    }
}