using System;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Resource
{
    /// <summary>
    /// The <see cref="IResource"/> implementation responsible for providing the Glimpse client the Glimpse logo.
    /// </summary>
    [Obsolete("This resource should not be requested anymore, but rather the LogosResource")]
    public class LogoResource : FileResource, IKey
    {
        internal const string InternalName = "glimpse_logo";

        private EmbeddedResourceInfo EmbeddedResourceInfo { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogoResource" /> class.
        /// </summary>
        public LogoResource()
        {
            Name = InternalName;
            EmbeddedResourceInfo = new EmbeddedResourceInfo(this.GetType().Assembly, "EmbeddedResources/glimpse_text_logo.png", "image/png");
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>
        /// The key. Only valid JavaScript identifiers should be used for future compatibility.
        /// </value>
        public string Key
        {
            get { return Name; }
        }

        /// <summary>
        /// Returns the embedded resource that represents the Glimpse text logo which will be returned during the execution of the <see cref="FileResource"/>
        /// </summary>
        /// <param name="context">The resource context</param>
        /// <returns>Information about the embedded Glimpse text logo</returns>
        protected override EmbeddedResourceInfo GetEmbeddedResourceInfo(IResourceContext context)
        {
            return EmbeddedResourceInfo;
        }
    }
}