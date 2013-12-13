using System;

namespace Glimpse.AspNet.Model
{
    public class CacheItemModel
    {
        private DateTime? expiresOn;

        public string Key { get; set; }

        public object Value { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? ExpiresOn { get
        {
            if (expiresOn == DateTime.MaxValue)
            {
                return null;
            }
            return expiresOn;
        }
            set { expiresOn = value; } 
        }

        public TimeSpan? SlidingExpiration { get; set; }
    }
}