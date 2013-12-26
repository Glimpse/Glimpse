using System.Collections.Generic;

namespace Glimpse.WindowsAzure.Storage.Infrastructure.Inspections
{
    public abstract class WindowsAzureStorageInspectorBase
        : IWindowsAzureStorageInspector
    {
        public virtual IEnumerable<string> Inspect()
        {
            return new string[] {};
        }

        public virtual IEnumerable<string> Inspect(WindowsAzureStorageTimelineMessage message)
        {
            return new string[] { };
        }
    }
}