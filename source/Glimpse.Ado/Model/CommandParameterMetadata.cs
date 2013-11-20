namespace Glimpse.Ado.Model
{
    public class CommandParameterMetadata
    {
        /// <summary>
        /// Gets or sets the name of the parameter.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of the parameter.
        /// </summary>
        /// <value>The value.</value>
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets the type of the parameter.
        /// </summary>
        /// <value>The type.</value>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the size of the parameter value.
        /// </summary>
        /// <value>The size.</value>
        public int Size { get; set; }
    }
}