using Glimpse.Core2.Extensibility;

namespace Glimpse.Test.Core2.TestDoubles
{
    [GlimpseTab(RequestContextType = typeof(DummyObjectContext))]
    public class DummyTab:ITab
    {
        public object GetData(ITabContext context)
        {
            throw new System.NotImplementedException("I am DummyTab");
        }

        public string Name
        {
            get { return "Dummy Tab"; }
        }
    }
}