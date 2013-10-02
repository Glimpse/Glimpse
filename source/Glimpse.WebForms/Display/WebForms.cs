using System;
using System.Collections.Generic;
using System.Linq; 
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Message;

namespace Glimpse.WebForms.Display
{ 
    [Obsolete]
    public class WebForms : IDisplay, ITabSetup, IKey
    {
        private const string InternalName = "WebForms";

        public string Name
        {
            get { return InternalName; }
        }

        public string Key
        {
            get { return InternalName; }
        }

        public object GetData(ITabContext context)
        { 
            var data = ProcessData(context.GetMessages<ITraceMessage>());
            return data;
        }

        private object ProcessData(IEnumerable<ITraceMessage> traceMessages)
        {
            var webFormsMessages = traceMessages.Where(x => x.Category == "ms").ToList();

            var loadingList = webFormsMessages.Where(x => x.Message.Contains("Load"));
            var loadingFirst = loadingList.First();
            var loadingLast = loadingList.Last();
            var loadingTime = loadingLast.FromFirst - loadingFirst.FromFirst;

            var renderingList = webFormsMessages.Where(x => x.Message.Contains("Render") || x.Message.Contains("State"));
            var renderingFirst = renderingList.First();
            var renderingLast = renderingList.Last();
            var renderingTime = renderingLast.FromFirst - renderingFirst.FromFirst;
             
            return new { loadingTime, renderingTime };
        }

        public void Setup(ITabSetupContext context)
        {
            context.PersistMessages<ITraceMessage>();
        }
    }
}
