using Glimpse.Core.Extensibility;

namespace Glimpse.Core.SerializationConverter
{
    public class BooleanConverter : SerializationConverter<bool>
    {
        public override object Convert(bool obj)
        {
            return obj.ToString();
        }
    }
}