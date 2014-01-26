using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;

namespace Glimpse.Core.Resource
{
    /// <summary>
    /// The <see cref="IResource"/> implementation responsible for providing all logos used in Glimpse.
    /// </summary>
    public class LogosResource : FileResource, IKey
    {
        internal const string InternalName = "glimpse_logos";
        private const string GlimpseTextLogoResourceName = "Glimpse.Core.EmbeddedResources.glimpse_text_logo.png";
        private const string GlimpseImageLogoResourceName = "Glimpse.Core.EmbeddedResources.glimpse_image_logo.png";
        private const string GlimpseFaviconResourceName = "Glimpse.Core.EmbeddedResources.glimpse_favicon.png";
        private const string GithubLogoResourceName = "Glimpse.Core.EmbeddedResources.github_logo.gif";
        private const string TwitterLogoResourceName = "Glimpse.Core.EmbeddedResources.twitter_logo.png";

        private readonly IDictionary<string, EmbeddedResourceInfo> embeddedResourceInfos = new Dictionary<string, EmbeddedResourceInfo>();

        /// <summary>
        /// Initializes a new instance of the <see cref="LogosResource" /> class.
        /// </summary>
        public LogosResource()
        {
            Name = InternalName;

            var assembly = this.GetType().Assembly;
            embeddedResourceInfos.Add("glimpse_text_logo", new EmbeddedResourceInfo(assembly, GlimpseTextLogoResourceName, "image/png"));
            embeddedResourceInfos.Add("glimpse_image_logo", new EmbeddedResourceInfo(assembly, GlimpseImageLogoResourceName, "image/png"));
            embeddedResourceInfos.Add("glimpse_favicon", new EmbeddedResourceInfo(assembly, GlimpseFaviconResourceName, "image/png"));
            embeddedResourceInfos.Add("github_logo", new EmbeddedResourceInfo(assembly, GithubLogoResourceName, "image/gif"));
            embeddedResourceInfos.Add("twitter_logo", new EmbeddedResourceInfo(assembly, TwitterLogoResourceName, "image/png"));
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
        /// Gets the required or optional parameters that a resource needs as processing input.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public override IEnumerable<ResourceParameterMetadata> Parameters
        {
            get { return new[] { ResourceParameter.LogoName, ResourceParameter.Hash }; }
        }

        /// <summary>
        /// Returns, based on the resource context, the embedded resource that represents a logo and will be returned during the execution of the <see cref="FileResource"/>
        /// </summary>
        /// <param name="context">The resource context</param>
        /// <returns>Information about the embedded resource that represents a logo</returns>
        protected override EmbeddedResourceInfo GetEmbeddedResourceInfo(IResourceContext context)
        {
            var logoname = context.Parameters.GetValueOrDefault(ResourceParameter.LogoName.Name);

            EmbeddedResourceInfo embeddedResourceInfo;
            if (!string.IsNullOrEmpty(logoname) && embeddedResourceInfos.TryGetValue(logoname, out embeddedResourceInfo))
            {
                return embeddedResourceInfo;
            }

            return null;
        }
    }
}