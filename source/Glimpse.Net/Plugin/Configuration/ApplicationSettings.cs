using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using Glimpse.Protocol;

namespace Glimpse.Net.Plugin.Configuration
{
    [GlimpsePlugin]
    public class ApplicationSettings:NameValueCollectionPlugin
    {
        public override string Name
        {
            get { return "ApplicationSettings"; }
        }

        public override IDictionary<string, string> GetData(HttpApplication application)
        {
            return Process(ConfigurationManager.AppSettings, application);
        }
    }
}
