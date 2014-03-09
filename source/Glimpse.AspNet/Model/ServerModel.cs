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
            GeneralVariables = new Dictionary<string, string>();
            SecurityRelatedVariables = new Dictionary<string, string>();
            
            foreach (var serverVariable in httpContext.Request.ServerVariables.ToDictionary())
            {
                string lowerCasedKey = serverVariable.Key.ToLower();

                if (lowerCasedKey.StartsWith("http_"))
                {
                    HttpVariables.Add(serverVariable.Key, serverVariable.Value);
                }
                else if (lowerCasedKey.StartsWith("cert_") || lowerCasedKey.StartsWith("https_"))
                {
                    SecurityRelatedVariables.Add(serverVariable.Key, serverVariable.Value);
                }
                else
                {
                    GeneralVariables.Add(serverVariable.Key, serverVariable.Value);
                }
            }
        }

        public IDictionary<string, string> HttpVariables { get; private set; }
        public IDictionary<string, string> GeneralVariables { get; private set; }
        public IDictionary<string, string> SecurityRelatedVariables { get; private set; }
    }
}
