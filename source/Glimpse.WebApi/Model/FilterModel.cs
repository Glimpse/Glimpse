using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glimpse.WebApi.Model
{
    public class FilterModel
    {
        public string Type { get; set; }

        public string Scope { get; set; }

        public bool AllowMultiple { get; set; }
    }
}
