using System.Collections.Generic;
using System.Net;

namespace Glimpse.WindowsAzure.Storage.Infrastructure.Inspections
{
    public class GeneralBestPracticesInspector
        : WindowsAzureStorageInspectorBase
    {
        public override IEnumerable<string> Inspect()
        {
            if (ServicePointManager.UseNagleAlgorithm)
            {
                yield return "For optimal Windows Azure Storage throughput, it is best to set ServicePointManager.UseNagleAlgorithm = false";
            }

            if (ServicePointManager.Expect100Continue)
            {
                yield return "For optimal Windows Azure Storage throughput, it is best to set ServicePointManager.Expect100Continue = false";
            }

            if (ServicePointManager.DefaultConnectionLimit < 100)
            {
                yield return "For optimal Windows Azure Storage throughput, it is best to set ServicePointManager.DefaultConnectionLimit = 100 (or more)";
            }
        }
    }
}