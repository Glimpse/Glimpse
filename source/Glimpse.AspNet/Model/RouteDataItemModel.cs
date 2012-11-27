namespace Glimpse.AspNet.Model 
{
    public class RouteDataItemModel
    {
        public RouteDataItemModel(string key, object defaultValue)
        {
            this.PlaceHolder = key;
            this.DefaultValue = defaultValue;
        }

        public string PlaceHolder { get; set; }

        public object DefaultValue { get; set; }

        public object ActualValue { get; set; }
    }
}