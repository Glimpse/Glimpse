using System;
using System.Linq;
using System.Reflection;

namespace Glimpse.Core.Extensibility
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class NuGetPackageAttribute : Attribute
    {
        public NuGetPackageAttribute() : this(null, null)
        {
        }
        
        public NuGetPackageAttribute(string id) : this(id, null)
        {
        }
        
        public NuGetPackageAttribute(string id, string version)
        {
            Id = id;
            Version = version;
        }

        private string Id { get; set; }

        private string Version { get; set; }

        public string GetId(Assembly assembly)
        {
            if (!string.IsNullOrEmpty(Id))
            {
                return Id;
            }

            return Id = assembly.GetName().Name;
        }

        public string GetVersion(Assembly assembly)
        {
            if (!string.IsNullOrEmpty(Version))
            {
                return Version;
            }

            var infoVersion = assembly.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false).Cast<AssemblyInformationalVersionAttribute>().SingleOrDefault();
            if (infoVersion != null)
            {
                return Version = infoVersion.InformationalVersion;
            }

            return Version = assembly.GetName().Version.ToString();
        }
    }
}