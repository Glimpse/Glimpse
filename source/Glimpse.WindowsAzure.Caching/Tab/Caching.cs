using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Message;
using Glimpse.WindowsAzure.Caching.Infrastructure;
using Glimpse.WindowsAzure.Caching.Models;

namespace Glimpse.WindowsAzure.Caching.Tab
{
    public class Caching
        : TabBase, IKey, ITabSetup
    {
        public override string Name
        {
            get { return "Windows Azure Caching"; }
        }

        public string Key
        {
            get { return "glimpse_waz_cache"; }
        }

        public void Setup(ITabSetupContext context)
        {
            context.PersistMessages<ITimelineMessage>();
        }

        public override object GetData(ITabContext context)
        {
            var timelineMessages = context.GetMessages<ITimelineMessage>().Where(m => m.EventName.StartsWith("WAZCache:"));

            var model = new CachingModel();

            if (timelineMessages != null)
            {
                // TODO: build useful data from this
                model.TotalSuccessful = timelineMessages.Count();
                return model;
            }

            return "No cache has been utilized for this request.";
        }
    }
}