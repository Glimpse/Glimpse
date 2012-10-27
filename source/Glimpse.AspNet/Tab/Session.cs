using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Glimpse.AspNet.Extensibility;
using Glimpse.AspNet.Model;
using Glimpse.Core.Extensibility;

namespace Glimpse.AspNet.Tab
{
    public class Session : AspNetTab, IDocumentation
    {
        public string DocumentationUri
        {
            // TODO: Update to proper Uri
            get { return "http://localhost/someUrl"; }
        }

        public override string Name
        {
            get { return "Session"; }
        }

        public override RuntimeEvent ExecuteOn
        {
            get { return RuntimeEvent.EndSessionAccess; }
        }

        public override object GetData(ITabContext context)
        {
            var requestContext = context.GetRequestContext<HttpContextBase>();

            var session = requestContext.Session;
            if (session == null || session.Count == 0)
            {
                return null;
            }

            var result = new List<SessionModel>(session.Count);
            foreach (var sessionKey in session.Keys)
            {
                var key = sessionKey.ToString();
                var value = session[key];
                var type = value != null ? value.GetType().ToString() : null;
                result.Add(new SessionModel { Key = key, Type = type, Value = value });
            }

            return result;
        }
    } 
}
