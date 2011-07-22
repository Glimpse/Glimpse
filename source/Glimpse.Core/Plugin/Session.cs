using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Plugin
{
    [GlimpsePlugin(SessionRequired = true)]
    internal class Session : IGlimpsePlugin, IProvideGlimpseHelp
    {
        public string Name
        {
            get { return "Session"; }
        }

        public object GetData(HttpContextBase context)
        {
            var session = context.Session;

            if (session == null) return null;

            var result = new List<object[]>
                             {
                                 new[] {"Key", "Value", "Type"}
                             };

            foreach (var key in session.Keys)
            {
                var keyString = key.ToString();
                var value = session[keyString];
                var type = value != null ? value.GetType().ToString() : null;
                result.Add(new[]{keyString, value, type});
            }

            if (result.Count > 1) return result;

            return null;
        }

        public void SetupInit()
        {
        }

        public string HelpUrl
        {
            get { return "http://getGlimpse.com/Help/Plugin/Session"; }
        }
    }
}