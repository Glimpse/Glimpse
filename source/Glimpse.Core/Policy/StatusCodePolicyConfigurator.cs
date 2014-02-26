using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Glimpse.Core.Framework;

namespace Glimpse.Core.Policy
{
    /// <summary>
    /// Implementation of an <see cref="IStatusCodePolicyConfigurator" />
    /// </summary>
    public class StatusCodePolicyConfigurator : AddRemoveClearItemsConfigurator<int>, IStatusCodePolicyConfigurator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusCodePolicyConfigurator" />
        /// </summary>
        public StatusCodePolicyConfigurator()
            : base("statusCodes", new IntComparer())
        {
            AddSupportedStatusCode(200);
            AddSupportedStatusCode(301);
            AddSupportedStatusCode(302);
        }

        /// <summary>
        /// Gets the supported status codes
        /// </summary>
        public IEnumerable<int> SupportedStatusCodes
        {
            get { return ConfiguredItems; }
        }

        /// <summary>
        /// Gets a boolean indicating whether there are supported status codes
        /// </summary>
        public bool ContainsSupportedStatusCodes
        {
            get { return ConfiguredItems.Count() != 0; }
        }

        /// <summary>
        /// Adds the given status codes to the list of supported status codes
        /// </summary>
        /// <param name="statusCodes">The status codes</param>
        public void AddSupportedStatusCodes(IEnumerable<int> statusCodes)
        {
            foreach (var statusCode in statusCodes)
            {
                AddSupportedStatusCode(statusCode);
            }
        }

        /// <summary>
        /// Adds the given status code to the list of supported status codes
        /// </summary>
        /// <param name="statusCode">The status code</param>
        public void AddSupportedStatusCode(int statusCode)
        {
            AddItem(statusCode);
        }

        /// <summary>
        /// Creates a int representing a status code
        /// </summary>
        /// <param name="itemNode">The <see cref="XmlNode"/> from which a status code is created</param>
        /// <returns>A status code</returns>
        protected override int CreateItem(XmlNode itemNode)
        {
            if (itemNode != null && itemNode.Attributes != null)
            {
                XmlAttribute statusCodeAttribute = itemNode.Attributes["statusCode"];
                if (statusCodeAttribute != null)
                {
                    return int.Parse(statusCodeAttribute.Value);
                }
            }
#warning CGI Add to resource file
            throw new GlimpseException("Could not find a 'statusCode' attribute");
        }

        private class IntComparer : IComparer<int>
        {
            public int Compare(int x, int y)
            {
                return x.CompareTo(y);
            }
        }
    }
}