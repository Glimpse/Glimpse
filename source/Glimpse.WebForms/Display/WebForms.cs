using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.AspNet.Extensions;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.WebForms.Inspector;

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

        public void Setup(ITabSetupContext context)
        {
            context.PersistMessages<PageLifeCycleMessage>();
        }

        public object GetData(ITabContext context)
        {
            var data = ProcessData(context.GetMessages<PageLifeCycleMessage>(), context);
            return data;
        }

        private object ProcessData(IEnumerable<PageLifeCycleMessage> webFormsMessages, ITabContext context)
        {
            var messages = webFormsMessages as IList<PageLifeCycleMessage> ?? webFormsMessages.ToList();
            var loadingList = messages.Where(x => x.EventName.Contains("Load")).ToList();
            var renderingList = messages.Where(x => x.EventName.Contains("Render") || x.EventName.Contains("State")).ToList();

            if (loadingList.Any() && renderingList.Any())
            {
                var loadingFirst = loadingList.First();
                var loadingLast = loadingList.Last();
                var loadingTime = loadingLast.Offset - loadingFirst.Offset;

                var renderingFirst = renderingList.First();
                var renderingLast = renderingList.Last();
                var renderingTime = renderingLast.Offset - renderingFirst.Offset;

                return new { loadingTime, renderingTime };
            }

            context.Logger.Warn("No page lifecycle messages found for {0}", context.GetHttpContext().Request.RawUrl);
            return null;
        }
    }
}
