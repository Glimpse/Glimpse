namespace Glimpse.Core.Plugin.Assist
{
    public class TabObjectRow : ITabObjectItem
    {
        internal object BaseKey { get; set; }

        internal object BaseValue { get; set; }

        public ITabObjectItem Key(object value)
        {
            BaseKey = value;
            return this;
        }

        public void Value(object value)
        {
            BaseValue = value;
        }
    }
}