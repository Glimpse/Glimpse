namespace Glimpse.WindowsAzure.Storage.Models
{
    public class StorageStatisticsModel
    {
        public int TotalStorageTx { get; set; }

        public int TotalBlobTx { get; set; }

        public int TotalQueueTx { get; set; }

        public int TotalTableTx { get; set; }

        public string TotalTrafficToStorage { get; set; }

        public string TotalTrafficFromStorage { get; set; }

        public string PricePerTenThousandPageViews { get; set; }
    }
}