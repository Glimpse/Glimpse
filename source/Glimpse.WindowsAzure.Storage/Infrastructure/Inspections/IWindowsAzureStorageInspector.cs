using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.Storage;

namespace Glimpse.WindowsAzure.Storage.Infrastructure.Inspections
{
    public interface IWindowsAzureStorageInspector
    {
        IEnumerable<string> Inspect();
        IEnumerable<string> Inspect(WindowsAzureStorageTimelineMessage message);
    }
}
