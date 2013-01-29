namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// <c>IDynamicClientScript</c>'s are a special type of <see cref="IClientScript"/> that sets the <c>&lt;script&gt;</c> tag's <c>src</c> attribute to the dynamically generated Uri for the <see cref="IResource"/> of a given name.
    /// </summary>
    public interface IDynamicClientScript : IClientScript
    {
        /// <summary>
        /// Gets the name of the <see cref="IResource"/> to dynamically generate a Uri for. 
        /// </summary>
        /// <remarks>
        /// <see cref="IResource"/> Uri generation is handled by implementations of <see cref="Glimpse.Core.Framework.ResourceEndpointConfiguration"/>.
        /// </remarks>
        /// <returns>Name of the <see cref="IResource"/> to link to.</returns>
        string GetResourceName();
    }
}