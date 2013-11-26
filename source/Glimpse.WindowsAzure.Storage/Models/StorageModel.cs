using System.Collections.Generic;

namespace Glimpse.WindowsAzure.Storage.Models
{
    public class StorageModel
    {
        public StorageModel()
        {
            Warnings = new List<object>();
            Statistics = new StorageStatisticsModel();
        }

        public StorageStatisticsModel Statistics { get; set; }

        public List<object> Warnings { get; set; }
    }
}