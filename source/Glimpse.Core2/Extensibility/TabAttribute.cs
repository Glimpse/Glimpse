using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;

namespace Glimpse.Core2.Extensibility
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TabAttribute:ExportAttribute, ITabMetadata
    {

        public TabAttribute(Type requestContextType, LifeCycleSupport lifeCycleSupport)
            : base(typeof(ITab))
        {
            Contract.Requires<ArgumentNullException>(requestContextType != null, "requestContextType");

            RequestContextType = requestContextType;
            LifeCycleSupport = lifeCycleSupport;
        }

        public TabAttribute(Type requestContextType): base(typeof(ITab))
        {
            Contract.Requires<ArgumentNullException>(requestContextType != null, "requestContextType");

            RequestContextType = requestContextType;
            LifeCycleSupport = LifeCycleSupport.EndRequest;
        }

        public TabAttribute():base(typeof(ITab))
        {
            RequestContextType = null;
            LifeCycleSupport = LifeCycleSupport.EndRequest;
        }



        public LifeCycleSupport LifeCycleSupport { get; set; }

        /// <summary>
        /// Gets or sets the type of the request context.
        /// </summary>
        /// <value>
        /// The type of the request context.
        /// </value>
        /// <remarks>
        /// The RequestContextType acts as a plugin filter. Glimpse will only execute plugins that have their RequestContextType specified if the current request's context type matches.
        /// </remarks>
        public Type RequestContextType { get; set; }
    }
}
