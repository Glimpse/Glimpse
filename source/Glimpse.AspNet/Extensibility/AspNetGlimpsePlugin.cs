using System.Web;
using Glimpse.Core2.Extensibility;

namespace Glimpse.AspNet.Extensibility
{
    public abstract class AspNetGlimpsePlugin : IGlimpsePlugin<HttpContextBase>
    {
        object IGlimpsePlugin<HttpContextBase>.GetData(IServiceLocator<HttpContextBase> locator)
        {
            return GetData((IServiceLocator) locator);
        }

        public abstract string Name { get; }
        public abstract object GetData(IServiceLocator locator);
    }
}