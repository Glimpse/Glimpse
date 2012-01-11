using System;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Test.Core2.TestDoubles
{
    [GlimpseTab(RequestContextType = typeof(DummyObjectContext))]
    public class DummySetupTab : ITab, IGlimpseTabSetup
    {
        public object GetData(ITabContext context)
        {
            throw new NotImplementedException("I am DummySetupTab");
        }

        public string Name
        {
            get { return "Dummy Setup Tab"; }
        }

        public void Setup()
        {
            throw new NotImplementedException("I am DummySetupTab");
        }
    }
}