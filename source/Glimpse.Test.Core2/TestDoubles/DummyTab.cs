using Glimpse.Core2.Extensibility;

namespace Glimpse.Test.Core2.TestDoubles
{
    [GlimpseTab]
    public class DummyTab:IGlimpseTab
    {
        public object GetData(IServiceLocator locator)
        {
            throw new System.NotImplementedException("I am DummyTab");
        }

        public string Name
        {
            get { return "Dummy Tab"; }
        }
    }
}