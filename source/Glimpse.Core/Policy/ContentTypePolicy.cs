using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Policy
{
    /// <summary>
    /// Policy which will set Glimpse's runtime policy to <c>Off</c> if a Http response's content type is not on the white list.
    /// </summary>
    public class ContentTypePolicy : IRuntimePolicy, IConfigurable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTypePolicy" /> class with an empty white list.
        /// </summary>
        public ContentTypePolicy()
            : this(new List<Tuple<string, RuntimePolicy>>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTypePolicy" /> class with the provided <paramref name="contentTypeWhiteList"/>.
        /// </summary>
        /// <param name="contentTypeWhiteList">The content type white list to validate against.</param>
        /// <exception cref="System.ArgumentNullException">Exception thrown if <paramref name="contentTypeWhiteList"/> is <c>null</c>.</exception>
        public ContentTypePolicy(IList<Tuple<string, RuntimePolicy>> contentTypeWhiteList)
        {
            if (contentTypeWhiteList == null)
            {
                throw new ArgumentNullException("contentTypeWhiteList");
            }

            ContentTypeWhiteList = contentTypeWhiteList;
        }

        /// <summary>
        /// Gets or sets the content type white list.
        /// </summary>
        /// <value>
        /// The content type white list to validate against.
        /// </value>
        public IList<Tuple<string, RuntimePolicy>> ContentTypeWhiteList { get; set; }

        /// <summary>
        /// Gets the point in an Http request lifecycle that a policy should execute.
        /// </summary>
        /// <value>
        /// The moment to execute, <see cref="AjaxPolicy"/> uses <c>EndRequest</c>.
        /// </value>
        public RuntimeEvent ExecuteOn
        {
            get { return RuntimeEvent.EndRequest; }
        }

        /// <summary>
        /// Executes the specified policy with the given context.
        /// </summary>
        /// <param name="policyContext">The policy context.</param>
        /// <returns>
        /// <c>On</c> if the response content type is contained on the white list, otherwise <c>Off</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Exception thrown if <paramref name="policyContext"/> is <c>null</c>.</exception>
        public RuntimePolicy Execute(IRuntimePolicyContext policyContext)
        {
            try
            {
                var contentType = policyContext.RequestMetadata.ResponseContentType.ToLowerInvariant();
                
                // support for the following content type strings: "text/html" & "text/html; charset=utf-8"
                var match = ContentTypeWhiteList.FirstOrDefault(ct => contentType.Contains(ct.Item1.ToLowerInvariant()));

                return match != null ? match.Item2 : RuntimePolicy.Off;
            }
            catch (Exception exception)
            {
                policyContext.Logger.Warn(Resources.ExecutePolicyWarning, exception, GetType());
                return RuntimePolicy.Off;
            }
        }

        /// <summary>
        /// Provides implementations an instance of <see cref="Section" /> to self populate any end user configuration options.
        /// </summary>
        /// <param name="section">The configuration section, <c>&lt;glimpse&gt;</c> from <c>web.config</c>.</param>
        /// <remarks>
        /// Populates the content type white list with values from <c>web.config</c>.
        /// A list of ratified Http status codes is available in <see href="http://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html">Section 10 of RFC 2616</see>, the Http version 1.1 specification.
        /// </remarks>
        /// <example>
        /// Configure the content type white list in <c>web.config</c> with the following entries:
        /// <code>
        /// <![CDATA[
        /// <glimpse defaultRuntimePolicy="On" endpointBaseUri="~/Glimpse.axd">
        ///     <runtimePolicies>
        ///         <contentTypes>
        ///             <!-- <clear /> clear to reset defaults -->
        ///             <add contentType="{media\type}" runtimePolicy="on" />
        ///         </contentTypes>
        ///     </runtimePolicies>
        /// </glimpse>
        /// ]]>
        /// </code>
        /// </example>
        public void Configure(Section section)
        {
            foreach (ContentTypeElement item in section.RuntimePolicies.ContentTypes)
            {
                ContentTypeWhiteList.Add(new Tuple<string, RuntimePolicy>(item.ContentType, item.RuntimePolicy));
            }
        }
    }
}