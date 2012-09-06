using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.ResourceResult;

namespace Glimpse.Core.Resource
{
    public class VersionCheckResource:IResource
    {
        internal const string InternalName = "glimpse_version_check";

        public string Name
        {
            get { return InternalName; }
        }
        public IEnumerable<ResourceParameterMetadata> Parameters
        {
            get { return new[] { ResourceParameter.VersionNumber, ResourceParameter.Timestamp, ResourceParameter.Callback }; }
        }
        public IResourceResult Execute(IResourceContext context)
        {
            var packages = new Dictionary<string, string>();
            
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var nugetPackage = assembly.GetCustomAttributes(typeof(NuGetPackageAttribute), false).Cast<NuGetPackageAttribute>().SingleOrDefault();
                if (nugetPackage == null)
                    continue;

                var version = nugetPackage.GetVersion(assembly);
                var id = nugetPackage.GetId(assembly);

                packages[id] = version;
            }

            var stamp = context.Parameters.GetValueOrDefault(ResourceParameter.Timestamp.Name);
            var callback = context.Parameters.GetValueOrDefault(ResourceParameter.Callback.Name);

            var data = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(stamp))
                data.Add("stamp", stamp);
            if (!string.IsNullOrEmpty(callback))
                data.Add("callback", callback);
            if (packages.Count > 0)
                data.Add("packages", packages);

            return
                new RedirectResourceResult(
                    @"//version.getglimpse.com/api/release/check{?packages*}{&stamp}{&callback}", data);
        }
    }
}