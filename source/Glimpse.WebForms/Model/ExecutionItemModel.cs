using System;

namespace Glimpse.WebForms.Model
{ 
    public class ExecutionItemModel
    {  
        public string Event { get; set; }

        public TimeSpan Duration { get; set; }

        public TimeSpan FromFirst { get; set; }
         
        public TimeSpan FromLast { get; set; }

        public int Ordinal { get; set; }
    }
}
