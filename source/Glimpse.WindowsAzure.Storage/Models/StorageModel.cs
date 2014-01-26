using System.Collections.Generic;

namespace Glimpse.WindowsAzure.Storage.Models
{
    public class StorageModel
    {
        public StorageModel()
        {
            Warnings = new List<StorageWarningModel>();
            Requests = new List<StorageRequestModel>();
            Statistics = new StorageStatisticsModel();
        }

        public StorageStatisticsModel Statistics { get; set; }

        public List<StorageWarningModel> Warnings { get; set; }
        public List<StorageRequestModel> Requests { get; set; }
    }
}