using System.Collections.Generic;
using Glimpse.AspNet.Extensibility;
using Glimpse.AspNet.Extensions;
using Glimpse.AspNet.Model;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Tab.Assist;

namespace Glimpse.AspNet.Tab
{
    public class Session : AspNetTab, IDocumentation, ITabLayout, IKey
    {
        private static readonly object Layout = TabLayout.Create()
                .Row(r =>
                {
                    r.Cell(0).AsKey().WidthInPixels(250);
                    r.Cell(1);
                    r.Cell(2).WidthInPercent(15);
                }).Build();

        public string DocumentationUri
        {
            get { return "http://getglimpse.com/Help/Session-Tab"; }
        }

        public override string Name
        {
            get { return "Session"; }
        }

        public string Key
        {
            get { return "glimpse_session"; }
        }

        public override RuntimeEvent ExecuteOn
        {
            get { return RuntimeEvent.EndSessionAccess; }
        }

        public object GetLayout()
        {
            return Layout;
        }

        public override object GetData(ITabContext context)
        {
            var requestContext = context.GetHttpContext();

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
                var type = value != null ? value.GetType() : null;
                result.Add(new SessionModel { Key = key, Type = type, Value = value });
            }

            return result;
        }
    } 
}
