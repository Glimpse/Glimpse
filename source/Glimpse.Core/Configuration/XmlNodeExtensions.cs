using System;
using System.Linq;
using System.Xml;
using Glimpse.Core.Framework;

namespace Glimpse.Core.Configuration
{
    internal static class XmlNodeExtensions
    {
        public static string GetAttributeValue(this XmlNode xmlNode, string attributeName, bool required, string defaultValue = null, string messageOnMissingAttribute = null)
        {
            XmlAttribute attribute = xmlNode.Attributes != null ? xmlNode.Attributes[attributeName] : null;
            if (attribute != null)
            {
                return attribute.Value;
            }
            else if (required)
            {
                throw new GlimpseException(messageOnMissingAttribute ?? "'" + attributeName + "' attribute missing");
            }

            return defaultValue;
        }

        public static Type GetTypeFromTypeAttribute(this XmlNode xmlNode, bool required, string messageOnMissingTypeAttribute = null)
        {
            XmlAttribute typeAttribute = xmlNode.Attributes != null ? xmlNode.Attributes["type"] : null;
            if (typeAttribute != null)
            {
                return Type.GetType(typeAttribute.Value, true, true);
            }
            else if (required)
            {
                throw new GlimpseException(messageOnMissingTypeAttribute ?? "type attribute missing");
            }

            return null;
        }

        public static TEnum GetAttributeValueAsEnum<TEnum>(this XmlNode xmlNode, string attributeName, bool required, TEnum defaultValue = default(TEnum), string messageOnMissingAttribute = null)
            where TEnum : struct
        {
            var attributeValue = GetAttributeValue(xmlNode, attributeName, required, defaultValue.ToString(), messageOnMissingAttribute);
            return (TEnum)Enum.Parse(typeof(TEnum), attributeValue);
        }
    }
}