using System;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Base implementation that allows simplified the implementation of a 
    /// tab. NOTE, this implementation is intended for by tabs that can  
    /// only be executed by a specific provider (i.e. ones bound to 
    /// HttpContextBase in the case of the ASP.NET provider). 
    /// </summary>
    /// <typeparam name="T">Type of the code provider that the tab is bound to</typeparam>
    public abstract class TabBase<T> : ITab
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
            get
            {
                return RuntimeEvent.EndRequest;
            }
        }

        /// <summary>
        /// Gets the type of the request context that the Tab relies on. NOTE,
        /// this implementation returns the type that the base was inherited with..
        /// </summary>
        /// <value>The type of the request context.</value>
        public Type RequestContextType
        {
            get { return typeof(T); }
        }

        /// <summary>
        /// Gets the data that should be shown in the UI.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>Object that will be shown.</returns>
        public abstract object GetData(ITabContext context);
    }
}