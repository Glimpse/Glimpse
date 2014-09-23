using System;
using System.Configuration;
using System.Xml;
using Glimpse.Core.Framework;

namespace Glimpse.Core.Configuration
{
    public class Section : ConfigurationSection
    {
        public Section()
        {
            var defaultXmlDocument = new XmlDocument();
            defaultXmlDocument.LoadXml(
                "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" +
                "<glimpse defaultRuntimePolicy=\"On\" endpointBaseUri=\"~/Glimpse.axd\" discoveryLocation=\"bin\\debug\">" +
                "<logging level=\"Trace\" />" +
                "</glimpse>");

            XmlContent = defaultXmlDocument.DocumentElement;
        }

        protected override void DeserializeSection(XmlReader reader)
        {
            var doc = new XmlDocument();
            doc.LoadXml(reader.ReadOuterXml());

            if (doc.DocumentElement == null)
            {
                throw new GlimpseException("There is no document element available to deserialize");
            }

            XmlContent = doc.DocumentElement;
            ConfigurationSettingsProviderType = XmlContent.GetTypeFromTypeAttribute(false);
            ExternalConfigurationFile = XmlContent.GetAttributeValue("config", false);
        }

        public XmlElement XmlContent { get; private set; }

        public string ExternalConfigurationFile { get; private set; }
        public Type ConfigurationSettingsProviderType { get; private set; }
    }
}