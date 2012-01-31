using System;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Test.Core2.TestDoubles
{
    [Tab(RequestContextType = typeof(DummyObjectContext))]
    public class DummySetupTab : ITab, ISetup
    {
        public object GetData(ITabContext context)
        {
            throw new NotSupportedException("I am DummySetupTab");
        }

        public string Name
        {
            get { return "Dummy Setup Tab"; }
        }

        public void Setup()
        {
            throw new NotSupportedException("I am DummySetupTab");
        }
    }
}