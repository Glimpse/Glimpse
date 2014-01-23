using System.Collections.Generic;
using System.Web;
﻿using Glimpse.Core.Extensions;

namespace Glimpse.AspNet.Model
{
    public class ServerModel
    {
        public ServerModel(HttpContextBase httpContext)
        {
            HttpVariables = new Dictionary<string, string>();
            OtherVariables = new Dictionary<string, string>();
            CertVariables = new Dictionary<string, string>();
            HttpsVariables = new Dictionary<string, string>();

            foreach (var serverVariable in httpContext.Request.ServerVariables.ToDictionary())
            {
                string lowerCasedKey = serverVariable.Key.ToLower();

                if (lowerCasedKey.StartsWith("http_"))
                {
                    HttpVariables.Add(serverVariable.Key, serverVariable.Value);
                }
                else if (lowerCasedKey.StartsWith("cert_"))
                {
                    CertVariables.Add(serverVariable.Key, serverVariable.Value);
                }
                else if (lowerCasedKey.StartsWith("https_"))
                {
                    HttpsVariables.Add(serverVariable.Key, serverVariable.Value);
                }
                else
                {
                    OtherVariables.Add(serverVariable.Key, serverVariable.Value);
                }
            }
        }

        public IDictionary<string, string> HttpVariables { get; private set; }
        public IDictionary<string, string> OtherVariables { get; private set; }
        public IDictionary<string, string> CertVariables { get; private set; }
        public IDictionary<string, string> HttpsVariables { get; private set; }
    }
}
