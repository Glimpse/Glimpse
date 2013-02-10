namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// <c>ResourceParameterMetadata</c> describes the parameters for an <see cref="IResource"/>.
    /// </summary>
    public class ResourceParameterMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceParameterMetadata" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="isRequired">if set to <c>true</c> [is required].</param>
        public ResourceParameterMetadata(string name, bool isRequired = true)
        {
            Name = name;
            IsRequired = isRequired;
        }

        /// <summary>
        /// Gets or sets the name of the parameter.
        /// </summary>
        /// <remarks>
        /// Names will typically become Html form fields or Uri query string parameter keys.
        /// </remarks>
        /// <value>
        /// The name. 
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this parameter is required.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is required; otherwise, <c>false</c>.
        /// </value>
        public bool IsRequired { get; set; }
    }
}