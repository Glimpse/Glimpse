namespace Glimpse.WebForms.Model
{
    public class DataBindModel
    {
        public string Event { get; private set; }

        public object Parameters { get; set; }

        public DataBindModel(string eventName, object parameters)
        {
            Event = eventName;
            Parameters = parameters;
        }
    }
}
