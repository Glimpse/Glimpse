using System;
using System.Web.Mvc;

namespace Glimpse.Test.Mvc.TestDoubles
{
    public class DummyValueProviderFactory : ValueProviderFactory
    {
        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            throw new NotSupportedException();
        }
    }
}