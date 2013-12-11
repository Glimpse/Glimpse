using System.Collections.Generic;

namespace Glimpse.AspNet.Model
{
    public class CacheModel
    {
        public CacheModel()
        {
            CacheItems = new List<CacheItemModel>(); 
            Configuration = new CacheConfigurationModel();
        }

        public CacheConfigurationModel Configuration { get; set; }

        public IList<CacheItemModel> CacheItems { get; set; } 
    }
}
