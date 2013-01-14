using Glimpse.AspNet.Extensibility;
using Glimpse.AspNet.Extensions;
using Glimpse.Core.Extensibility;

namespace Glimpse.Test.Integration.Site
{
    public class IntegrationTestTab : AspNetTab, IKey
    {
        public const string Expected = "__intTestKey";

        public override string Name
        {
            get { return "Integration"; }
        }

        public string Key 
        {
            get { return "test-tab"; }
        }

        public override object GetData(ITabContext context)
        {
            var httpContext = context.GetHttpContext();

            return string.Format("<div id='__intTestId'>{0}</div>", httpContext.Items[Expected]);
        }
    }
}