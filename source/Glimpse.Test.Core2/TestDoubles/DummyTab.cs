using Glimpse.Core2.Extensibility;

namespace Glimpse.Test.Core2.TestDoubles
{
    public class DummyTab:TabBase<DummyObjectContext>
    {
        public override object GetData(ITabContext context)
        {
            throw new System.NotSupportedException("I am DummyTab");
        }

        public override string Name
        {
            get { return "Dummy Tab"; }
        }
    }
}