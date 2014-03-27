using System;
using System.Configuration;
using System.Xml;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.Core.Configuration
{
    /// <summary>
    /// The Glimpse configuration node that contains the necessary information to allow Glimpse to automatically find and load types at runtime.
    /// </summary>
    /// <remarks>
    /// <c>DiscoverableCollectionElement</c> is used to configure many types, including: <see cref="IClientScript"/>, <see cref="IInspector"/>, <see cref="ISerializationConverter"/>, <see cref="ITab"/> and <see cref="IRuntimePolicy"/>'s
    /// </remarks>
    [Obsolete("As it only deserializes to get the custom xml, it doesn't make sense to keep on using a specialized class for it, Glimpse Section should be an xml document")]
    public class DiscoverableCollectionElement : ConfigurationElement
    {
        /// <summary>
        /// Custom deserializes the xml element by turning it into a collection of <see cref="CustomConfiguration"/>
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/></param>
        /// <param name="serializeCollectionKey">The serialize collection key</param>
        protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(reader.ReadOuterXml());

            if (doc.DocumentElement == null)
            {
                throw new GlimpseException("There is no document element available to deserialize");
            }

            XmlContent = doc.DocumentElement;
        }

        public XmlElement XmlContent { get; private set; }
    }
}