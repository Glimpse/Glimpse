using System;
using System.Collections.Generic;
using System.Web;
using Glimpse.Protocol;

namespace Glimpse.Net.Plugin.Asp
{
    [GlimpsePlugin(SessionRequired = true)]
    public class Session:IGlimpsePlugin
    {
        public string Name
        {
            get { return "Session"; }
        }

        public IDictionary<string, string> GetData(HttpApplication application)
        {
            var sessionData = new Dictionary<string, string>();
            var session = application.Session;
            foreach (var key in session.Keys)
            {
                sessionData.Add(key.ToString(), session[key.ToString()].ToString());
            }

            if (sessionData.Count == 0) return null;

            return sessionData;
        }
    }
}
