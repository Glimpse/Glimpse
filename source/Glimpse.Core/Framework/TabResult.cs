using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// Contains the results of executing an <see cref="ITab"/>, along with its key.
    /// </summary>
    public class TabResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TabResult" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="data">The data.</param>
        public TabResult(string name, object data)
        {
            Data = data;
            Name = name;
        }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <remarks>The Data property will be serialized by the Glimpse runtime, as such, objects stored in Data should be serializable.</remarks>
        /// <value>
        /// The data.
        /// </value>
        public object Data { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
    }
}