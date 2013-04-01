using System;

namespace Glimpse.Test.Integration.Site.Models
{
    public class CustomModel
    {
        public CustomModel(Guid guid)
        {
            Guid = guid;
            Timestamp = DateTime.Now;
        }

        public Guid Guid { get; set; }

        public DateTime Timestamp { get; private set; }
    }
}