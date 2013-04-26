using System;
using System.Linq;
using System.Reflection;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// An attribute used to identify the corresponding NuGet package and Id for an assembly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public class NuGetPackageAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetPackageAttribute" /> class.
        /// </summary>
        public NuGetPackageAttribute() : this(null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetPackageAttribute" /> class.
        /// </summary>
        /// <param name="id">The id.</param>
        public NuGetPackageAttribute(string id) : this(id, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetPackageAttribute" /> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="version">The version.</param>
        public NuGetPackageAttribute(string id, string version)
        {
            Id = id;
            Version = version; 
        }

        private string Id { get; set; }

        private string Version { get; set; }

        private string AssemblyName { get; set; }

        /// <summary>
        /// Setup Attribute with Assembly context
        /// </summary>
        /// <param name="assembly"></param>
        public void Initialize(Assembly assembly)
        {
            if (string.IsNullOrEmpty(Version))
            {
                var infoVersion = assembly.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false).Cast<AssemblyInformationalVersionAttribute>().SingleOrDefault();
                Version = infoVersion != null ? infoVersion.InformationalVersion : assembly.GetName().Version.ToString();
            }

            if (string.IsNullOrEmpty(Id))
            {
                Id = assembly.GetName().Name;    
            }

            AssemblyName = assembly.GetName().FullName;
        }

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <returns>The NuGet package Id for the initialized assembly.</returns>
        public string GetId()
        {
            return Id; 
        }

        /// <summary>
        /// Gets the version.
        /// </summary> 
        /// <returns>The NuGet package version for the initialized assembly.</returns>
        public string GetVersion()
        { 
            return Version;  
        }

        /// <summary>
        /// Returns the full name of the assembly that the attribute is in.
        /// </summary>
        /// <returns>Full name value</returns>
        public string GetAssemblyName()
        {
            return AssemblyName;
        }
    }
}