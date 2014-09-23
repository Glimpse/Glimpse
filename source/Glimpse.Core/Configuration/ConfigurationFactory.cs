using System;
using System.Configuration;
using System.IO;
using System.Xml;
using Glimpse.Core.Framework;

namespace Glimpse.Core.Configuration
{
    /// <summary>
    /// Factory for <see cref="IConfiguration" /> instances
    /// </summary>
    public static class ConfigurationFactory
    {
        /// <summary>
        /// Creates an <see cref="IConfiguration" /> based on a <see cref="Section" /> resolved with the "glimpse" configuration section name
        /// </summary>
        /// <param name="resourceEndpointConfiguration">The resource endpoint configuration</param>
        /// <param name="persistenceStore">The persistence store</param>
        /// <param name="currentGlimpseRequestIdTracker">An optional <see cref="ICurrentGlimpseRequestIdTracker"/> which will default to <see cref="CallContextCurrentGlimpseRequestIdTracker"/></param>
        /// <returns>The <see cref="IConfiguration"/></returns>
        public static IConfiguration Create(
            ResourceEndpointConfiguration resourceEndpointConfiguration,
            IPersistenceStore persistenceStore,
            ICurrentGlimpseRequestIdTracker currentGlimpseRequestIdTracker = null)
        {
            return new Configuration(
                DetermineConfigurationSettingsProvider().GetConfigurationSettings(
                    resourceEndpointConfiguration,
                    persistenceStore,
                    currentGlimpseRequestIdTracker ?? new CallContextCurrentGlimpseRequestIdTracker()));
        }

        private static IConfigurationSettingsProvider DetermineConfigurationSettingsProvider()
        {
            // get the section from config, and if it does not exist use a default section
            var glimpseSection = ConfigurationManager.GetSection("glimpse") as Section ?? new Section();

            // if that section defines a configuration settings provider, then use that one
            if (glimpseSection.ConfigurationSettingsProviderType != null)
            {
                if (typeof(IConfigurationSettingsProvider).IsAssignableFrom(glimpseSection.ConfigurationSettingsProviderType))
                {
                    return (IConfigurationSettingsProvider)Activator.CreateInstance(glimpseSection.ConfigurationSettingsProviderType);
                }
                else
                {
                    throw new GlimpseException("The configured configuration settings provider '" + glimpseSection.ConfigurationSettingsProviderType.FullName + "' does not implement '" + typeof(IConfigurationSettingsProvider).FullName + "'.");
                }
            }

            var configurationDocument = new XmlDocument();

            // since there is no configuration settings provider specified (and we don't allow one in an external configuration file)
            // we will check whether there is an external configuration file specified, if so that one is used, otherwise the content
            // specified by the Section (whether it was specified or the default)

            // maybe we need to build in some retry mechanism to read the content of the file
            if (!string.IsNullOrEmpty(glimpseSection.ExternalConfigurationFile) && File.Exists(glimpseSection.ExternalConfigurationFile))
            {
                configurationDocument.Load(glimpseSection.ExternalConfigurationFile);
            }
            else
            {
                configurationDocument.LoadXml(glimpseSection.XmlContent.OuterXml);
            }

            // we'll use the built-in xml provider
            return new ConfigurationSettingsXmlProvider(configurationDocument);
        }
    }
}