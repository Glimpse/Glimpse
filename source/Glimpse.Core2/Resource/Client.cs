using System.Collections.Generic;
using System.Reflection;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;

namespace Glimpse.Core2.Resource
{
    [Resource]
    public class Client:IResource
    {
        internal const string InternalName = "glimpse.js";
        internal string ResourceName = "Glimpse.Core2.glimpseClient.js";

        public string Name
        {
            get { return InternalName; }
        }

        public IEnumerable<string> ParameterKeys
        {
            get { return new[]{ResourceParameterKey.VersionNumber }; }
        }

        public IResourceResult Execute(IResourceContext context)
        {
            var assembly = Assembly.GetExecutingAssembly();

            //TODO: Rename from Core2 to Core
            using (var resourceStream = assembly.GetManifestResourceStream(ResourceName))
            {
                if (resourceStream != null)
                {
                    var content = new byte[resourceStream.Length];
                    resourceStream.Read(content, 0, content.Length);
                    var cacheDuration = 150*24*60*60; //150 days * hours * minutes * seconds
                    return new FileResourceResult(content, @"application/x-javascript", cacheDuration, CacheSetting.Public);
                }
            }

            return new StatusCodeResourceResult(404);
        }
    }
}