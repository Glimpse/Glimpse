using System;
using System.Web.Mvc;

namespace Glimpse.Test.Mvc3.TestDoubles
{
    public class DummyValueProviderFactory : ValueProviderFactory
    {
        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            throw new NotSupportedException();
        }
    }
}