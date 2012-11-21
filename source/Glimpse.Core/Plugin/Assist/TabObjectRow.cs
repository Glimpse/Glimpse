namespace Glimpse.Core.Plugin.Assist
{
    public class TabObjectRow : ITabObjectItem, ITabStyleValue
    {
        internal object BaseKey { get; set; }

        internal object BaseValue { get; set; }

        public ITabObjectItem Key(object value)
        {
            BaseKey = value;
            return this;
        }

        public ITabStyleValue Value(object value)
        {
            BaseValue = value;
            return this;
        }

        public void ApplyValueStyle(string format)
        {
            BaseValue = format.FormatWith(BaseValue);
        }
    }
}