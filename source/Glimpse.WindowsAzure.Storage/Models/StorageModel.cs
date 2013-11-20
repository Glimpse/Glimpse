using System.Collections.Generic;

namespace Glimpse.WindowsAzure.Storage.Models
{
    public class StorageModel
    {
        public StorageModel()
        {
            Warnings = new List<object>();
        }

        public int TotalStorageTx { get; set; }
        public int TotalBlobTx { get; set; }
        public int TotalQueueTx { get; set; }
        public int TotalTableTx { get; set; }
        public string TotalTrafficToStorage { get; set; }
        public string TotalTrafficFromStorage { get; set; }
        public string PricePerTenThousandPageViews { get; set; }
        public List<object> Warnings { get; set; }
    }
}