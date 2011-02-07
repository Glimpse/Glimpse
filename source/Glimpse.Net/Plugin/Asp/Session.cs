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

        public object GetData(HttpApplication application)
        {
            var result = new List<object[]>
                             {
                                 new[]{"Key", "Value", "Type"}
                             };

            var session = application.Session;

            foreach (var key in session.Keys)
            {
                var value = session[key.ToString()];
                var type = value.GetType();

                result.Add(new[] { key.ToString(), value, type.ToString()});
            }

            if (result.Count == 0) return null;

            return result;
        }
    }
}
