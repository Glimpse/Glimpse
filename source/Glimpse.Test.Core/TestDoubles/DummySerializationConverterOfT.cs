using Glimpse.Core.Extensibility;

namespace Glimpse.Test.Core.TestDoubles
{
    public class DummySerializationConverterOfT:SerializationConverter<DummyObjectContext>
    {
        public override object Convert(DummyObjectContext obj)
        {
            throw new System.NotSupportedException();
        }
    }
}