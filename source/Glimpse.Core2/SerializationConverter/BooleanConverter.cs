using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.SerializationConverter
{
    public class BooleanConverter : SerializationConverter<bool>
    {
        public override object Convert(bool obj)
        {
            return obj.ToString();
        }
    }
}