using System.Collections.Generic;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// A class which describes Glimpse system metadata, as required by a client.
    /// </summary>
    public class GlimpseMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GlimpseMetadata" /> class.
        /// </summary>
        public GlimpseMetadata()
        {
            Tabs = new Dictionary<string, object>();
            Resources = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets or sets the running version of Glimpse.
        /// </summary>
        /// <value>
        /// The running version of Glimpse.
        /// </value>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the hash used for HTTP cache busting.
        /// </summary>
        /// <value>
        /// The <seealso href="http://en.wikipedia.org/wiki/Cyclic_redundancy_check">CRC32</seealso> hash of Glimpse's configuration.
        /// </value>
        public string Hash { get; set; }

        /// <summary>
        /// Gets or sets the collection of tab specific metadata.
        /// </summary>
        /// <value>
        /// The tab's metadata.
        /// </value>
        public IDictionary<string, object> Tabs { get; set; }

        /// <summary>
        /// Gets or sets the collection resources keys and their corresponding Uri templates.
        /// </summary>
        /// <value>
        /// The resources keys and Uri templates.
        /// </value>
        public IDictionary<string, string> Resources { get; set; }
    }
}