using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using Glimpse.Core.Framework;

namespace Glimpse.Core.Policy
{
    /// <summary>
    /// Implementation of an <see cref="IUriPolicyConfigurator" />
    /// </summary>
    public class UriPolicyConfigurator : AddRemoveClearItemsConfigurator<Regex>, IUriPolicyConfigurator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UriPolicyConfigurator" />
        /// </summary>
        public UriPolicyConfigurator()
            : base("uris", new RegexComparer())
        {
            AddUriPatternToIgnore("__browserLink/requestData");
        }

        /// <summary>
        /// Gets the uri patterns that will be ignored
        /// </summary>
        public IEnumerable<Regex> UriPatternsToIgnore
        {
            get { return ConfiguredItems; }
        }

        /// <summary>
        /// Gets a boolean indicating whether there are uri patterns that will be ignored
        /// </summary>
        public bool ContainsUriPatternsToIgnore
        {
            get { return ConfiguredItems.Count() != 0; }
        }

        /// <summary>
        /// Adds the given uri patterns to the the list of uri patterns to ignore
        /// </summary>
        /// <param name="uriPatternsToIgnore">The uri patterns to ignore</param>
        public void AddSupportedStatusCodes(IEnumerable<string> uriPatternsToIgnore)
        {
            foreach (var uriPatternToIgnore in uriPatternsToIgnore)
            {
                AddUriPatternToIgnore(uriPatternToIgnore);
            }
        }

        /// <summary>
        /// Adds the given uri pattern to the list of uri patterns that will be ignored
        /// </summary>
        /// <param name="uriPattern">The uri pattern</param>
        public void AddUriPatternToIgnore(string uriPattern)
        {
            AddItem(new Regex(uriPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase));
        }

        /// <summary>
        /// Creates a <see cref="Regex"/> representing a uri pattern to ignore
        /// </summary>
        /// <param name="itemNode">The <see cref="XmlNode"/> from which a uri regex pattern is created</param>
        /// <returns>A uri regex pattern</returns>
        protected override Regex CreateItem(XmlNode itemNode)
        {
            if (itemNode != null && itemNode.Attributes != null)
            {
                XmlAttribute uriAttribute = itemNode.Attributes["uri"];
                if (uriAttribute != null)
                {
                    return new Regex(uriAttribute.Value, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                }
            }
#warning CGI Add to resource file
            throw new GlimpseException("Could not find a 'uri' attribute");
        }

        private class RegexComparer : IComparer<Regex>
        {
            public int Compare(Regex x, Regex y)
            {
                return string.Compare(x.ToString(), y.ToString(), System.StringComparison.Ordinal);
            }
        }
    }
}