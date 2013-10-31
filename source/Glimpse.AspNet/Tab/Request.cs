using Glimpse.AspNet.Extensibility;
using Glimpse.AspNet.Extensions;
using Glimpse.AspNet.Model;
using Glimpse.Core.Extensibility;

namespace Glimpse.AspNet.Tab
{
    public class Request : AspNetTab, IDocumentation, IKey
    {
        /// <summary>
        /// Gets the name that will show in the tab.
        /// </summary>
        /// <value>The name.</value>
        public override string Name
        {
            get { return "Request"; }
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key. Only valid JavaScript identifiers should be used for future compatibility.</value>
        public string Key 
        {
            get { return "glimpse_request"; }
        }

        /// <summary>
        /// Gets the documentation URI.
        /// </summary>
        /// <value>The documentation URI.</value>
        public string DocumentationUri
        {
            get { return "http://getglimpse.com/Help/Request-Tab"; }
        }

        /// <summary>
        /// Gets the data that should be shown in the UI.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>Object that will be shown.</returns>
        public override object GetData(ITabContext context)
        {
            var httpContext = context.GetHttpContext();

            return new RequestModel(httpContext);
        }
    }
}
