namespace Glimpse.Core2.Framework
{
    public class TabResult
    {
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <remarks>The Data property will be serialized by the Glimpse runtime, as such, objects stored in Data should be serializable.</remarks>
        /// <value>
        /// The data.
        /// </value>
        public object Data { get; set; }

        public string Name { get; set; }

        public TabResult(string name, object data)
        {
            Data = data;
            Name = name;
        }
    }
}