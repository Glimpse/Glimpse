using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace Glimpse.AspNet.Model
{
    public class CacheModel
    {
        public CacheModel()
        {
            CacheItems = new List<CacheItemModel>(); 
        }

        public long EffectivePercentagePhysicalMemoryLimit { get; set; }
        public long EffectivePrivateBytesLimit { get; set; }
        public IList<CacheItemModel> CacheItems { get; set; }

    }

    public class CacheItemModel
    {
        public string Key { get; set; }
        public object Value { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ExpiresOn { get; set; }
        public TimeSpan? SlidingExpiration { get; set; }
    }
}
