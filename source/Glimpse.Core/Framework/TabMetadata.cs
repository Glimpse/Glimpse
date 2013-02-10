namespace Glimpse.Core.Framework
{
    /// <summary>
    /// Contains any metadata associated with a given tab.
    /// </summary>
    public class TabMetadata
    {
        /// <summary>
        /// Gets or sets the documentation Uri.
        /// </summary>
        /// <value>
        /// The documentation URI.
        /// </value>
        public string DocumentationUri { get; set; }

        /// <summary>
        /// Gets or sets the layout override instructions.
        /// </summary>
        /// <value>
        /// The layout.
        /// </value>
        public object Layout { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance has metadata.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has metadata; otherwise, <c>false</c>.
        /// </value>
        public bool HasMetadata 
        { 
            get
            {
                return !string.IsNullOrEmpty(DocumentationUri) || this.Layout != null;
            } 
        }
    }
}