namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Enum that specifies the possible order placement for <see cref="IClientScript"/> 
    /// </summary>
    public enum ScriptOrder
    {
        /// <summary>
        /// The include before client interface script
        /// </summary>
        IncludeBeforeClientInterfaceScript,
        /// <summary>
        /// The client interface script
        /// </summary>
        /// <remarks>
        /// This should not be used by non system scripts
        /// </remarks>
        ClientInterfaceScript,
        /// <summary>
        /// The include after client interface script
        /// </summary>
        IncludeAfterClientInterfaceScript,
        /// <summary>
        /// The request metadata script
        /// </summary>
        /// <remarks>
        /// This should not be used by non system scripts
        /// </remarks>
        RequestMetadataScript,
        /// <summary>
        /// The include before request data script
        /// </summary>
        IncludeBeforeRequestDataScript,
        /// <summary>
        /// The request data script
        /// </summary>
        /// <remarks>
        /// This should not be used by non system scripts
        /// </remarks>
        RequestDataScript,
        /// <summary>
        /// The include after request data script
        /// </summary>
        IncludeAfterRequestDataScript,
    }
}