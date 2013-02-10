namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Definition for how the layout of a given object can be controlled. When
    /// implementing a layout, <see cref="Glimpse.Core.Tab.Assist.TabLayout"/>
    /// can be used to make creating the required objects easier.
    /// </summary>
    public interface ITabLayout
    {
        /// <summary>
        /// Gets the layout which controls the layout.
        /// </summary>
        /// <returns>Object that dictates the layout.</returns>
        object GetLayout();
    }
}