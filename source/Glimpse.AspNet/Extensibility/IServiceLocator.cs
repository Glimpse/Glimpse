using System.Web;
using Glimpse.Core2.Extensibility;

namespace Glimpse.AspNet.Extensibility
{
    public interface IServiceLocator:IServiceLocator<HttpContextBase>
    {
        
    }
}
