using System;
using System.IO;
using System.Web;
using System.Xml.Linq;

namespace Glimpse.Elmah.Plumbing
{
	public class ConfigurationReaderForIntegratedPipelineMode : IConfigurationReader
	{
		public string GetPathFor<THandler>()
		{
			// This sucks, but there's no other way to read the system.webServer section
			var configFilePath = HttpRuntime.AppDomainAppPath + "Web.config";
			var configFileInfo = new FileInfo(configFilePath);
			if (!configFileInfo.Exists)
				return null;

			var configFile = File.ReadAllText(configFilePath);
			if (string.IsNullOrWhiteSpace(configFile))
				return null;

			var configFileDocument = XDocument.Parse(configFile);
			if (configFileDocument.Root == null)
				return null;

			var webServerSection = configFileDocument.Root.Element("system.webServer");
			if (webServerSection == null)
				return null;

			var handlersSection = webServerSection.Element("handlers");
			if (handlersSection == null)
				return null;

			var handlers = handlersSection.Elements();
		    foreach (var handler in handlers)
			{
				var handlerTypeAttribute = handler.Attribute(XName.Get("type"));
				if (handlerTypeAttribute == null)
					continue;

				var handlerType = Type.GetType(handlerTypeAttribute.Value);
                if (!typeof(THandler).IsAssignableFrom(handlerType)) 
					continue;

				var handlerPathAttribute = handler.Attribute(XName.Get("path"));
				if (handlerPathAttribute != null)
				{
					return handlerPathAttribute.Value;
				}
			}

			return null;
		}
	}
}