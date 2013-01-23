using System;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// <para>
    /// Definition for an object which will be represented in the UI as a Tab. To 
    /// operate you simply need to implement this interface and return an object 
    /// graph that you would like to view.
    /// </para>
    /// <para>
    /// When returning data, you can either return the object which represents the 
    /// Glimpse protocol or return a typed view model. It is recommended that you 
    /// use typed view models, as it is this object that will be stored in by the 
    /// Data Store. If this is done, it means that these typed objects can be queried
    /// at a latter stage. That said, if you are after simplicity, you can return 
    /// objects in the shape the Glimpse Protocol requires.
    /// </para>
    /// <para>
    /// If you do decide to work with a typed view model you can convert this into 
    /// an objects in the shape the Glimpse Protocol by implementing <see cref="ISerializationConverter"/>
    /// targeting the type of the view model.
    /// </para>
    /// <para>
    /// Alternatively, if you after simplicity and return an object which represents  
    /// the Glimpse protocol, you can use <see cref="Glimpse.Core.Tab.Assist.TabObject"/>
    /// and <see cref="Glimpse.Core.Tab.Assist.TabSection"/> as helpers.
    /// </para>
    /// <para>
    /// Additional functionality can be injected into via additionally implementing 
    /// the following interfaces:
    /// - <seealso cref="IKey"/>: Key that will be used to identify the tab within the client.
    /// - <seealso cref="IDocumentation"/>: URI that will describe where the documentation for the tab can be found.
    /// - <seealso cref="ITabLayout"/>: Control how the layout of the content should look.
    /// - <seealso cref="ITabSetup"/>: Allows tab to inject any hooks it needs within the system.
    /// - <seealso cref="ISerializationConverter"/>: Allow complex objects to be converted into the Glimpse protocol. 
    /// </para>
    /// </summary>
    public interface ITab
    {
        /// <summary>
        /// Gets the name that will show in the tab.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; }

        /// <summary>
        /// Gets when the <see cref="ITab.GetData"/> method should run.
        /// </summary>
        /// <value>The execute on.</value>
        RuntimeEvent ExecuteOn { get; }

        /// <summary>
        /// Gets the type of the request context that the Tab relies on. If 
        /// returns null, the tab can be used in any context.
        /// </summary>
        /// <value>The type of the request context.</value>
        Type RequestContextType { get; }

        /// <summary>
        /// Gets the data that should be shown in the UI.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>Object that will be shown.</returns>
        object GetData(ITabContext context);
    }
}
