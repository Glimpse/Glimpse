using System.Web;
using Glimpse.Core.Extensibility;
using System.Web.Http.Controllers;

namespace Glimpse.WebApi.Core.Extensibility
{
    public abstract class WebApiTab : TabBase<HttpContextBase>
    {
    }

    public abstract class WebApiRequestTab : TabBase<HttpRequestContext>
    {
    }

}