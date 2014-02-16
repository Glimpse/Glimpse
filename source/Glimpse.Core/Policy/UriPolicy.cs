using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Policy
{
    /// <summary>
    /// Policy which will set Glimpse's runtime policy to <c>Off</c> if a Http request's Uri matches a pattern in the black list.
    /// </summary>
    public class UriPolicy : IRuntimePolicy, IConfigurable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UriPolicy" /> class with an empty black list.
        /// </summary>
        public UriPolicy() : this(new List<Regex>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UriPolicy" /> class with the provided <paramref name="uriBlackList"/>.
        /// </summary>
        /// <param name="uriBlackList">The Uri black list to validate against. Regular expressions are also supported in the black list.</param>
        /// <exception cref="System.ArgumentNullException">Exception thrown if <paramref name="uriBlackList"/> is <c>null</c>.</exception>
        public UriPolicy(IList<Regex> uriBlackList)
        {
            if (uriBlackList == null)
            {
                throw new ArgumentNullException("uriBlackList");
            }

            UriBlackList = uriBlackList;
        }

        /// <summary>
        /// Gets the point in an Http request lifecycle that a policy should execute.
        /// </summary>
        /// <value>
        /// The moment to execute, <see cref="AjaxPolicy"/> uses <c>BeginRequest</c>.
        /// </value>
        public RuntimeEvent ExecuteOn
        {
            get { return RuntimeEvent.BeginRequest; }
        }

        /// <summary>
        /// Gets or sets the Uri black list.
        /// </summary>
        /// <value>
        /// The Uri black list to validate against.
        /// </value>
        public IList<Regex> UriBlackList { get; set; }

        /// <summary>
        /// Executes the specified policy with the given context.
        /// </summary>
        /// <param name="policyContext">The policy context.</param>
        /// <returns>
        /// <c>On</c> if the request Uri is contained not matched in the black list, otherwise <c>Off</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Exception thrown if <paramref name="policyContext"/> is <c>null</c>.</exception>
        public RuntimePolicy Execute(IRuntimePolicyContext policyContext)
        {
            try
            {
                if (UriBlackList.Count == 0)
                {
                    return RuntimePolicy.On;
                }

                var uri = policyContext.RequestMetadata.RequestUri.AbsoluteUri;

                if (UriBlackList.Any(regex => regex.IsMatch(uri)))
                {
                    return RuntimePolicy.Off;
                }

                return RuntimePolicy.On;
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
        /// Populates the Uri black list with values from <c>web.config</c>.
        /// </remarks>
        /// <example>
        /// Configure the Uri black list in <c>web.config</c> with the following entries:
        /// <code>
        /// <![CDATA[
        /// <glimpse defaultRuntimePolicy="On" endpointBaseUri="~/Glimpse.axd">
        ///     <runtimePolicies>
        ///         <uris>
        ///             <!-- <clear /> clear to reset defaults -->
        ///             <add regex="{regular expression or uri}" />
        ///         </uris>
        ///     </runtimePolicies>
        /// </glimpse>
        /// ]]>
        /// </code>
        /// </example>
        public void Configure(Section section)
        {
            UriBlackList.Add(new Regex("__browserLink/requestData")); 
            foreach (RegexElement item in section.RuntimePolicies.Uris)
            {
                UriBlackList.Add(item.Regex);
            }
        }
    }
}