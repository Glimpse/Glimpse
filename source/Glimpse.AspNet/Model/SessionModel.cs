using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glimpse.AspNet.Model
{
    public class SessionModel
    {
        public string Key { get; set; }
        
        public object Value { get; set; }

        public string Type { get; set; }
    }
}
