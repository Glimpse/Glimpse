using System;

namespace Glimpse.WindowsAzure.Storage.Models
{
    public class StorageRequestModel
    {
        public string Service { get; set; }
        public string Operation { get; set; }
        public int ResponseCode { get; set; }
        public long ResponseSize { get; set; }
        public string Url { get; set; }
        public TimeSpan Duration { get; set; }
        public TimeSpan Offset { get; set; }
        public object _metadata { get; set; }
    }
}