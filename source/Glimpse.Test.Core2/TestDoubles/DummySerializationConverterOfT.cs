using Glimpse.Core2.Extensibility;

namespace Glimpse.Test.Core2.TestDoubles
{
    public class DummySerializationConverterOfT:SerializationConverter<DummyObjectContext>
    {
        public override object Convert(DummyObjectContext obj)
        {
            throw new System.NotSupportedException();
        }
    }
}