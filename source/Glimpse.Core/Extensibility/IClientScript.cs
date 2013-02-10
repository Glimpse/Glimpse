namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// <c>IClientScript</c> injects <c>&lt;script&gt;</c> tags into page responses.
    /// </summary>
    /// <remarks>
    /// There are three sub-types of <c>IClientScript</c>: 
    ///  <list type="number">
    ///   <item>
    ///    <term><see cref="IStaticClientScript"/></term>
    ///    <description><c>IStaticClientScript</c>'s create <c>&lt;script&gt;</c> tag's with a <c>src</c> attribute pointing to a specific Uri.</description>
    ///   </item>
    ///   <item>
    ///    <term><see cref="IDynamicClientScript"/></term>
    ///    <description><c>IDynamicClientScript</c>'s create <c>&lt;script&gt;</c> tag's with a <c>src</c> attribute pointing to the Uri of a specific type of <see cref="IResource"/>.</description>
    ///   </item>
    ///  </list>
    /// </remarks>
    public interface IClientScript
    {
        /// <summary>
        /// Gets the sorting order in which a <c>&lt;script&gt;</c> tag will be injected a page response, relative to other implementations of <see cref="IClientScript"/>.
        /// </summary>
        /// <value>Any value from the <see cref="ScriptOrder"/> enumeration.</value>
        /// <remarks>
        /// Multiple <see cref="IClientScript"/>'s with the same <see cref="ScriptOrder"/> will be sorted in an indeterminate order.
        /// </remarks>
        ScriptOrder Order { get; }
    }
}