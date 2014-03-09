﻿using Glimpse.AspNet.Extensibility;
using Glimpse.AspNet.Extensions;
﻿using Glimpse.AspNet.Model;
﻿using Glimpse.Core.Extensibility;

namespace Glimpse.AspNet.Tab
{
    public class Server : AspNetTab, IDocumentation, IKey, ILayoutControl
    {
        public override string Name
        {
            get { return "Server"; }
        }

        public string Key
        {
            get { return "glimpse_server"; }
        }

        public bool KeysHeadings
        {
            get { return true; }
        }

        public string DocumentationUri
        {
            get { return "http://getglimpse.com/Help/ServerTab"; }
        }

        public override object GetData(ITabContext context)
        {
            var httpContext = context.GetHttpContext();
            var serverModel = new ServerModel(httpContext);
            return serverModel;
        }

    }
}