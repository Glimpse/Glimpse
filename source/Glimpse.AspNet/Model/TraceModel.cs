using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glimpse.Core.Tab.Assist;

namespace Glimpse.AspNet.Model
{
    public class TraceModel
    {
        public string Category { get; set; }

        public string Message { get; set; }

        public double FromFirst { get; set; }

        public double FromLast { get; set; }

        public int IndentLevel { get; set; }
    }
}
