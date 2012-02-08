using System;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Test.Core2.TestDoubles
{
    public class DummySetupTab : TabBase<DummyObjectContext>, ISetup
    {
        public override object GetData(ITabContext context)
        {
            throw new NotSupportedException("I am DummySetupTab");
        }

        public override string Name
        {
            get { return "Dummy Setup Tab"; }
        }

        public void Setup()
        {
            throw new NotSupportedException("I am DummySetupTab");
        }
    }
}