using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;

namespace Glimpse.Core2.Extensibility
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GlimpseTabAttribute:ExportAttribute, IGlimpseTabMetadata
    {

        public GlimpseTabAttribute(Type requestContextType, LifeCycleSupport lifeCycleSupport)
            : base(typeof(IGlimpseTab))
        {
            Contract.Requires<ArgumentNullException>(requestContextType != null, "requestContextType");

            RequestContextType = requestContextType;
            LifeCycleSupport = lifeCycleSupport;
        }

        public GlimpseTabAttribute(Type requestContextType): base(typeof(IGlimpseTab))
        {
            Contract.Requires<ArgumentNullException>(requestContextType != null, "requestContextType");

            RequestContextType = requestContextType;
            LifeCycleSupport = LifeCycleSupport.EndRequest;
        }

        public GlimpseTabAttribute():base(typeof(IGlimpseTab))
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
