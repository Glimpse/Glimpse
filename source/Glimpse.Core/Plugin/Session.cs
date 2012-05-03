using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Plugin
{
    [GlimpsePlugin(SessionRequired = true)]
    internal class Session : IGlimpsePlugin, IProvideGlimpseHelp
    {
        internal static string[] Header = new[] {"Key", "Value", "Type", "Size (bytes)"};

        public string Name
        {
            get { return "Session"; }
        }

        public object GetData(HttpContextBase context)
        {
            var session = context.Session;

            if (session == null) return null;

            var result = new List<object[]> { Header };

            foreach (var key in session.Keys)
            {
                var keyString = key.ToString();
                var value = session[keyString];
                var type = value != null ? value.GetType().ToString() : null;
                result.Add(new[]{keyString, value, type,GetObjectSize(value)});
            }

            if (result.Count > 1)
            {
                var sessionSize = result.Sum(o => o[3] as long?);
                result.Add(new object[] {"Total Session Size", "", "(calculated)", sessionSize});

                return result;
            }

            return null;
        }

        public void SetupInit()
        {
        }

        public string HelpUrl
        {
            get { return "http://getGlimpse.com/Help/Plugin/Session"; }
        }

        private static long GetObjectSize(object obj)
        {
            if (obj == null)
                return 0;

            using (var stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, obj);
                return stream.Length;
            }
        }
    }
}