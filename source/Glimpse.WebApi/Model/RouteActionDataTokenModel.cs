using System;
using System.Collections.Generic;

namespace Glimpse.WebApi.Model
{
    public class RouteActionDataTokenModel
    {
        public string ActionName { get; set; }

        public string ControllerName { get; set; }

        public IEnumerable<string> SupportedHttpMethods { get; set; }

        public string ReturnType { get; set; }

        public object ResultConvertor { get; set; }

        public object Properties { get; set; }
    }
}
