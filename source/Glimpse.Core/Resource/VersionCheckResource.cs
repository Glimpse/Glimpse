using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Framework;
using Glimpse.Core.ResourceResult;

namespace Glimpse.Core.Resource
{
    /// <summary>
    /// The <see cref="IResource"/> implementation responsible for providing the Glimpse client the newest version number of Glimpse.
    /// </summary>
    public class VersionCheckResource : IResource, IKey
    {
        internal const string InternalName = "glimpse_version_check";
        private const int OneDay = 86400;

        /// <summary>
        /// Gets the name of the resource.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        /// <remarks>
        /// Resource name's should be unique across a given web application. If multiple <see cref="IResource" /> implementations contain the same name, Glimpse may throw an exception resulting in an Http 500 Server Error.
        /// </remarks>
        public string Name
        {
            get { return InternalName; }
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
        public IEnumerable<ResourceParameterMetadata> Parameters
        {
            get
            {
                return new[] { ResourceParameter.VersionNumber, ResourceParameter.Timestamp, ResourceParameter.Callback };
            }
        }

        /// <summary>
        /// Executes the specified resource with the specific context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <see cref="IResourceResult" /> that can be executed when the Http response is ready to be returned.
        /// </returns>
        public IResourceResult Execute(IResourceContext context)
        {
            var packages = new Dictionary<string, string>();
            
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var nugetPackage = assembly.GetCustomAttributes(typeof(NuGetPackageAttribute), false).Cast<NuGetPackageAttribute>().SingleOrDefault();
                if (nugetPackage == null)
                {
                    continue;
                }

                var version = nugetPackage.GetVersion(assembly);
                var id = nugetPackage.GetId(assembly);

                packages[id] = version;
            }

            var stamp = context.Parameters.GetValueOrDefault(ResourceParameter.Timestamp.Name);
            var callback = context.Parameters.GetValueOrDefault(ResourceParameter.Callback.Name);

            var data = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(stamp))
            {
                data.Add("stamp", stamp);
            }

            if (!string.IsNullOrEmpty(callback))
            {
                data.Add("callback", callback);
            }

            if (packages.Count > 0)
            {
                data.Add("packages", packages);
            }

            var domain = ConfigurationManager.AppSettings["GlimpseVersionCheckAPIDomain"];
            
            if (string.IsNullOrEmpty(domain))
            {
                domain = "version.getglimpse.com";
            }

            return new CacheControlDecorator(OneDay, CacheSetting.Public, new RedirectResourceResult(@"//" + domain + "/Api/Version/Check{?packages*}{&stamp}{&callback}", data));
        }
    }
}