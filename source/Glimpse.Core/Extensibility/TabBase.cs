using System;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Base implementation that allows simplified the implementation of a 
    /// tab. NOTE, this implementation is intended for by tabs that can 
    /// execute on any provider (i.e. aren't bound to HttpContextBase in 
    /// the case of the ASP.NET provider). Most tabs wont be tied to a 
    /// specific provider.
    /// </summary>
    public abstract class TabBase : ITab
    {
        /// <summary>
        /// Gets the name that will show in the tab.
        /// </summary>
        /// <value>The name.</value>
        public abstract string Name { get; }

        /// <summary>
        /// Gets when the <see cref="ITab.GetData" /> method should run.
        /// </summary>
        /// <value>The execute on.</value>
        public virtual RuntimeEvent ExecuteOn
        {
            get { return RuntimeEvent.EndRequest; }
        }

        /// <summary>
        /// Gets the type of the request context that the Tab relies on. NOTE, 
        /// this implementation returns null, hence tabs implementing this base  
        /// can used by any provider.
        /// </summary>
        /// <value>The type of the request context.</value>
        public Type RequestContextType
        {
            get { return null; }
        }

        /// <summary>
        /// Gets the data that should be shown in the UI.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>Object that will be shown.</returns>
        public abstract object GetData(ITabContext context);
    }
}