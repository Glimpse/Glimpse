using System;
using System.Configuration;
using System.Web.Configuration;

namespace Glimpse.Elmah.Plumbing
{
	public class ConfigurationReaderForClassicMode : IConfigurationReader
	{
        public string GetPathFor<THandler>()
		{
			var httpHandlersSection = (HttpHandlersSection) ConfigurationManager.GetSection("system.web/httpHandlers");
			if (httpHandlersSection == null)
				return null;

			var handlers = httpHandlersSection.Handlers;
			if (handlers == null)
				return null;

			foreach (HttpHandlerAction handler in handlers)
			{
				var handlerType = Type.GetType(handler.Type);
				if (typeof(THandler).IsAssignableFrom(handlerType))
				{
					return handler.Path;
				}
			}

			return null;
		}
	}
}