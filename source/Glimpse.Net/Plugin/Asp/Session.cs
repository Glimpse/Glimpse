using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glimpse.Net.Extensibility;

namespace Glimpse.Net.Plugin.Asp
{
    [GlimpsePlugin(SessionRequired = true)]
    internal class Session : IGlimpsePlugin
    {
        public string Name
        {
            get { return "Session"; }
        }

        public object GetData(HttpApplication application)
        {
            var session = application.Session;

            var result = new List<object[]>
                             {
                                 new[] {"Key", "Value", "Type"}
                             };

            result.AddRange(from object key in session.Keys
                            let value = session[key.ToString()]
                            let type = value.GetType()
                            select new[] {key.ToString(), value, type.ToString()});

            if (result.Count > 1) return result;

            return null;
        }

        public void SetupInit(HttpApplication application)
        {
            throw new NotImplementedException();
        }
    }
}