using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Message;
using Glimpse.WindowsAzure.Storage.Infrastructure;
using Glimpse.WindowsAzure.Storage.Models;

namespace Glimpse.WindowsAzure.Storage.Tab
{
    public class Storage
        : TabBase, IKey, ITabSetup
    {
        public override string Name
        {
            get { return "Windows Azure Storage"; }
        }

        public string Key
        {
            get { return "glimpse_waz_storage"; }
        }

        public void Setup(ITabSetupContext context)
        {
            context.PersistMessages<ITimelineMessage>();
        }

        public override object GetData(ITabContext context)
        {
            var timelineMessages = context.GetMessages<ITimelineMessage>()
                .Where(m => m.EventName.StartsWith("WAZStorage:")).Cast<WindowsAzureStorageTimelineMessage>();

            var model = new StorageModel();

            if (timelineMessages != null)
            {
                model.TotalStorageTx = timelineMessages.Count();
                model.TotalBlobTx = timelineMessages.Count(m => m.EventName.StartsWith("WAZStorage:Blob"));
                model.TotalTableTx = timelineMessages.Count(m => m.EventName.StartsWith("WAZStorage:Table"));
                model.TotalQueueTx = timelineMessages.Count(m => m.EventName.StartsWith("WAZStorage:Queue"));
                model.TotalTrafficToStorage = timelineMessages.Sum(m => m.RequestSize).ToBytesHuman();
                model.TotalTrafficFromStorage = timelineMessages.Sum(m => m.ResponseSize).ToBytesHuman();
                model.PricePerTenThousandPageViews = string.Format("${0}", (model.TotalStorageTx * 1000 * 0.0000001 + timelineMessages.Sum(m => m.ResponseSize) * 10000 * (0.12 / 1024 / 1024 / 1024)));
                model.Warnings.AddRange(timelineMessages.Where(m => !string.IsNullOrEmpty(m.Warning)).Select(m => new
                {
                    RequestUrl = m.Url,
                    Warning = m.Warning
                }));
                return model;
            }

            return "No storage transactions have been utilized for this request.";
        }
    }
}