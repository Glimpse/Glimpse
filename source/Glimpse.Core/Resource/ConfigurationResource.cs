using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Antlr4.StringTemplate;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Core.Metadata;
using Glimpse.Core.ResourceResult;
using Glimpse.Core.Support;

namespace Glimpse.Core.Resource
{
    /// <summary>
    /// The <see cref="IResource"/> implementation responsible for providing the Glimpse configuration page, which is usually where a user turns Glimpse on and off.
    /// </summary>
    public class ConfigurationResource : IPrivilegedResource, IKey, IDependOnResources
    {
        internal const string InternalName = "glimpse_config";
        private readonly string[] resourceDependencies;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationResource"/> class
        /// </summary>
        public ConfigurationResource()
        {
            resourceDependencies = new[]
            {
                LogosResource.InternalName,
                ConfigurationStyleResource.InternalName,
                ConfigurationScriptResource.InternalName    
            };
        }

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
        /// Gets the required or optional parameters that a resource needs as processing input.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public IEnumerable<ResourceParameterMetadata> Parameters
        {
            get { return Enumerable.Empty<ResourceParameterMetadata>(); }
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
        /// Determines whether or not the resource depends on the given resource
        /// </summary>
        /// <param name="resourceName">The internal name of the resource</param>
        /// <returns>Boolean indicating whether or not the resource depends on the given resource</returns>
        public bool DependsOn(string resourceName)
        {
            return resourceDependencies.Contains(resourceName);
        }

        /// <summary>
        /// Executes the specified resource with the specific context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <see cref="IResourceResult" /> that can be executed when the Http response is ready to be returned.
        /// </returns>
        /// <exception cref="System.NotSupportedException">Throws a <see cref="NotSupportedException"/> since this is a <see cref="IPrivilegedResource"/>.</exception>
        public IResourceResult Execute(IResourceContext context)
        {
            throw new NotSupportedException(string.Format(Resources.PrivilegedResourceExecuteNotSupported, GetType().Name));
        }

        /// <summary>
        /// Executes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="requestResponseAdapter">The request response adapter.</param>
        /// <returns>
        /// A <see cref="IResourceResult" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Exception thrown if either <paramref name="context"/> or <paramref name="configuration"/> are <c>null</c>.</exception>
        /// <remarks>
        /// Use of <see cref="IPrivilegedResource" /> is reserved.
        /// </remarks>
        /// 
        public IResourceResult Execute(IResourceContext context, IConfiguration configuration, IRequestResponseAdapter requestResponseAdapter)
        {
            const string glimpseConfigurationResourceName = "Glimpse.Core.EmbeddedResources." + InternalName + ".html";
            Stream glimpseConfigurationResourceStream = this.GetType().Assembly.GetManifestResourceStream(glimpseConfigurationResourceName);
            if (glimpseConfigurationResourceStream == null)
            {
                throw new GlimpseException("Could not load embedded resource '" + glimpseConfigurationResourceName + "'");
            }

            string glimpseConfigurationTemplateContent = new StreamReader(glimpseConfigurationResourceStream).ReadToEnd();
            Template glimpseConfigurationTemplate = new Template(new TemplateGroup('$', '$'), glimpseConfigurationTemplateContent);

            glimpseConfigurationTemplate.Add("glimpseRuntimeVersion", configuration.Version);
             
            var resources = configuration.PersistenceStore.GetMetadata().GetResources();
            var logosResource = resources[LogosResource.InternalName].Replace("{&" + ResourceParameter.Hash.Name + "}", string.Empty);
            var logoNamePlaceholder = "{" + ResourceParameter.LogoName.Name + "}";
            glimpseConfigurationTemplate.Add("glimpseFaviconUri", logosResource.Replace(logoNamePlaceholder, "glimpse_favicon"));
            glimpseConfigurationTemplate.Add("glimpseLogoUri", logosResource.Replace(logoNamePlaceholder, "glimpse_image_logo"));
            glimpseConfigurationTemplate.Add("githubLogoUri", logosResource.Replace(logoNamePlaceholder, "github_logo"));
            glimpseConfigurationTemplate.Add("twitterLogoUri", logosResource.Replace(logoNamePlaceholder, "twitter_logo"));

            glimpseConfigurationTemplate.Add("configurationStyleUri", resources[ConfigurationStyleResource.InternalName].Replace("{&" + ResourceParameter.Hash.Name + "}", string.Empty));
            glimpseConfigurationTemplate.Add("configurationScriptUri", resources[ConfigurationScriptResource.InternalName].Replace("{&" + ResourceParameter.Hash.Name + "}", string.Empty));

            // Duplicate resources
            var duplicateResources = DetectDuplicateResources(configuration.Resources).ToList();
            glimpseConfigurationTemplate.Add("hasDuplicateResources", duplicateResources.Count != 0);
            glimpseConfigurationTemplate.Add("duplicateResources", duplicateResources.Select((duplicateResource, index) => new { Name = duplicateResource, IsNotFirstDuplicate = index != 0 }));

            var packages = FindPackages();

            // Tabs
            Func<ITab, object> createTabItemDisplay = registeredTab => new
            {
                registeredTab.Name,
                Type = registeredTab.GetType().FullName,
                registeredTab.ExecuteOn,
                AssemblyName = registeredTab.GetType().Assembly.GetName().Name
            };

            glimpseConfigurationTemplate.Add(
                "tabsByPackage",
                GroupItemsByPackage(configuration.Tabs.OrderBy(x => x.Name), packages, createTabItemDisplay).ToArray());

            // Runtime Policies
            Func<IRuntimePolicy, object> createRuntimePolicyItemDisplay = registeredRuntimePolicy =>
            {
                string warningMessage = registeredRuntimePolicy.GetType().FullName == "Glimpse.AspNet.Policy.LocalPolicy" ? "*This policy means that Glimpse won't run remotely.*" : string.Empty;
                return new
                {
                    Type = registeredRuntimePolicy.GetType().FullName,
                    registeredRuntimePolicy.ExecuteOn,
                    AssemblyName = registeredRuntimePolicy.GetType().Assembly.GetName().Name,
                    WarningMessage = warningMessage,
                    HasWarningMessage = !string.IsNullOrEmpty(warningMessage)
                };
            };
            glimpseConfigurationTemplate.Add(
                "runtimePoliciesByPackage",
                GroupItemsByPackage(configuration.RuntimePolicies.OrderBy(x => x.GetType().FullName), packages, createRuntimePolicyItemDisplay).ToArray());

            // Inspectors
            Func<IInspector, object> createInspectorItemDisplay = inspector => new
            {
                Type = inspector.GetType().FullName,
                AssemblyName = inspector.GetType().Assembly.GetName().Name
            };

            glimpseConfigurationTemplate.Add(
                "inspectorsByPackage",
                GroupItemsByPackage(configuration.Inspectors.OrderBy(x => x.GetType().FullName), packages, createInspectorItemDisplay).ToArray());

            // Resources
            Func<IResource, object> createResourceItemDisplay = resource => new
            {
                resource.Name,
                Type = resource.GetType().FullName,
                Parameters = resource.Parameters != null ? string.Join(", ", resource.Parameters.Select(parameter => string.Format("{0} ({1})", parameter.Name, parameter.IsRequired)).ToArray()) : string.Empty,
                HasDuplicate = duplicateResources.Contains(resource.Name)
            };

            glimpseConfigurationTemplate.Add(
                "resourcesByPackage",
                GroupItemsByPackage(configuration.Resources.OrderBy(x => x.Name), packages, createResourceItemDisplay).ToArray());

            // Client Scripts
            Func<IClientScript, object> createClientScriptItemDisplay = clientScript => new
            {
                Type = clientScript.GetType().FullName,
                clientScript.Order
            };

            glimpseConfigurationTemplate.Add(
                "clientScriptsByPackage",
                GroupItemsByPackage(configuration.ClientScripts.OrderBy(x => x.GetType().FullName), packages, createClientScriptItemDisplay).ToArray());

            // Remainder
            glimpseConfigurationTemplate.Add("frameworkProviderType", requestResponseAdapter.GetType().FullName);
            glimpseConfigurationTemplate.Add("htmlEncoderType", configuration.HtmlEncoder.GetType().FullName);
            glimpseConfigurationTemplate.Add("loggerType", configuration.Logger.GetType().FullName);
            glimpseConfigurationTemplate.Add("persistenceStoreType", configuration.PersistenceStore.GetType().FullName);
            glimpseConfigurationTemplate.Add("resourceEndpointType", configuration.ResourceEndpoint.GetType().FullName);
            glimpseConfigurationTemplate.Add("serializerType", configuration.Serializer.GetType().FullName);
            glimpseConfigurationTemplate.Add("defaultResourceType", configuration.DefaultResource.GetType().FullName);
            glimpseConfigurationTemplate.Add("defaultResourceName", configuration.DefaultResource.Name);
            glimpseConfigurationTemplate.Add("defaultRuntimePolicyType", configuration.DefaultRuntimePolicy.GetType().FullName);
            glimpseConfigurationTemplate.Add("proxyFactoryType", configuration.ProxyFactory.GetType().FullName);
            glimpseConfigurationTemplate.Add("messageBrokerType", configuration.MessageBroker.GetType().FullName);
            glimpseConfigurationTemplate.Add("endpointBaseUri", configuration.EndpointBaseUri);

            // Registered Packages
            var nuGetPackageDiscoveryResult = NuGetPackageDiscoverer.Discover();
            glimpseConfigurationTemplate.Add(
                "registeredNuGetPackages",
                nuGetPackageDiscoveryResult.FoundNuGetPackages.Select(
                    registeredNuGetPackage => new
                    {
                        Id = registeredNuGetPackage.GetId(),
                        Version = registeredNuGetPackage.GetVersion()
                    }));

            Assembly[] nonProcessableAssemblies = nuGetPackageDiscoveryResult.NonProcessableAssemblies;
            glimpseConfigurationTemplate.Add("hasNonProcessableAssemblies", nonProcessableAssemblies.Length != 0);
            if (nonProcessableAssemblies.Length != 0)
            {
                glimpseConfigurationTemplate.Add("nonProcessableAssemblies", nonProcessableAssemblies.Select(nonProcessableAssembly => nonProcessableAssembly.FullName).ToArray());
            }

            return new HtmlResourceResult(glimpseConfigurationTemplate.Render());
        }

        private static IEnumerable<object> GroupItemsByPackage<T>(IEnumerable<T> items, IDictionary<string, PackageItemDetail> packages, Func<T, object> createItemDisplay)
        {
            var groupedItemsByPackage = GroupItems(items, packages);

            var itemsByPackage = new List<object>();

            foreach (var groupedItemsForPackage in groupedItemsByPackage)
            {
                var package = groupedItemsForPackage.Value;

                itemsByPackage.Add(new
                {
                    PackageName = package.Package.Name,
                    PackageVersion = !string.IsNullOrEmpty(package.Package.Version) ? string.Format("({0})", package.Package.Version) : string.Empty,
                    ContainedItems = package.Items.Select(createItemDisplay).ToArray()
                });
            }

            return itemsByPackage;
        }

        private static IDictionary<string, PackageItem<T>> GroupItems<T>(IEnumerable<T> items, IDictionary<string, PackageItemDetail> packages)
        {
            var result = new SortedDictionary<string, PackageItem<T>>();
            var otherPackage = new PackageItemDetail { Name = "Other", Assembly = string.Empty };

            if (items != null)
            {
                foreach (var item in items)
                {
                    PackageItemDetail package;
                    if (!packages.TryGetValue(item.GetType().Assembly.FullName, out package))
                    {
                        package = otherPackage;
                    }

                    PackageItem<T> entry;
                    if (!result.TryGetValue(package.Assembly, out entry))
                    {
                        entry = result[package.Assembly] = new PackageItem<T>();
                        entry.Package = package;
                    }

                    entry.Items.Add(item);
                }
            }

            return result;
        }

        private static IDictionary<string, PackageItemDetail> FindPackages()
        {
            var result = new Dictionary<string, PackageItemDetail>();
            var packages = NuGetPackage.GetRegisteredPackages();

            foreach (var package in packages)
            {
                var name = package.GetAssemblyName();
                result[name] = new PackageItemDetail { Name = package.GetId(), Version = package.GetVersion(), Assembly = name };
            }

            return result;
        }

        private static IEnumerable<string> DetectDuplicateResources(IEnumerable<IResource> resources)
        {
            return resources.GroupBy(x => x.Name).Where(x => x.Count() > 1).Select(x => x.Key);
        }

        private class PackageItem<T>
        {
            public PackageItem()
            {
                Items = new List<T>();
            }

            public PackageItemDetail Package { get; set; }

            public IList<T> Items { get; private set; }
        }

        private class PackageItemDetail
        {
            public string Name { get; set; }

            public string Version { get; set; }

            public string Assembly { get; set; }
        }
    }
}