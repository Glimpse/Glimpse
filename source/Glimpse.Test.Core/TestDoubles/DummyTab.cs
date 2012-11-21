using Glimpse.Core.Extensibility;

namespace Glimpse.Test.Core.TestDoubles
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