namespace Glimpse.AspNet.Model
{
    public class CacheItemModel
    {
        public string Key { get; set; }

        public object Value { get; set; }

        public object CreatedOn { get; set; }

        public object ExpiresOn { get; set; }

        public object SlidingExpiration { get; set; }
    }
}