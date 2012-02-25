using System;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Test.Core2.TestDoubles
{
    public class DummyTabSetupTab : TabBase<DummyObjectContext>, ITabSetup
    {
        public override object GetData(ITabContext context)
        {
            throw new NotSupportedException("I am DummySetupTab");
        }

        public override string Name
        {
            get { return "Dummy Setup Tab"; }
        }

        public void Setup(ITabSetupContext context)
        {
            throw new NotSupportedException("I am DummySetupTab");
        }
    }
}