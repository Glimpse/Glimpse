using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glimpse.Core.Plugin.Assist;

namespace Glimpse.AspNet.Model
{
    public class TraceModel
    {
        public FormattingKeywordEnum Category { get; set; }

        public string Message { get; set; }

        public double FromFirst { get; set; }

        public double FromLast { get; set; }

        public int IndentLevel { get; set; }
    }
}
