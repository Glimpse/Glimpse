using System;
using Glimpse.Core2;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Test.Core2
{
    [GlimpseTab(RequestContextType = typeof(string))]
    public class TestTab : IGlimpseTab, IGlimpsePluginSetup
    {
        public object GetData(IServiceLocator locator)
        {
            throw new NotImplementedException();
        }

        public string Name
        {
            get { return "Test Plugin"; }
        }

        public void Setup()
        {
            throw new NotImplementedException();
        }
    }
}