using System;
using System.ComponentModel.Composition;

namespace Glimpse.Core2.Extensibility
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GlimpseTabAttribute:ExportAttribute, IGlimpseTabMetadata
    {
        public GlimpseTabAttribute():base(typeof(IGlimpseTab))
        {
            RequestContextType = null;
            LifeCycleSupport = LifeCycleSupport.EndRequest;
        }

        public GlimpseTabAttribute(Type requestContextType): base(typeof(IGlimpseTab))
        {
            RequestContextType = requestContextType;
            LifeCycleSupport = LifeCycleSupport.EndRequest;
        }

        public GlimpseTabAttribute(Type requestContextType, LifeCycleSupport lifeCycleSupport)
            : base(typeof(IGlimpseTab))
        {
            RequestContextType = requestContextType;
            LifeCycleSupport = lifeCycleSupport;
        }

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
        public LifeCycleSupport LifeCycleSupport { get; set; }
    }
}
