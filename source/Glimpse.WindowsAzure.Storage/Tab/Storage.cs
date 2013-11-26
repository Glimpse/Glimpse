using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Message;
using Glimpse.WindowsAzure.Storage.Infrastructure;
using Glimpse.WindowsAzure.Storage.Infrastructure.Inspections;
using Glimpse.WindowsAzure.Storage.Models;

namespace Glimpse.WindowsAzure.Storage.Tab
{
    public class Storage : TabBase, IKey, ITabSetup, ILayoutControl
    {
        public override string Name
        {
            get { return "Azure Storage"; }
        }

        public string Key
        {
            get { return "glimpse_waz_storage"; }
        }

        public bool KeysHeadings
        {
            get { return true; }
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
                model.Statistics.TotalStorageTx = timelineMessages.Count();
                model.Statistics.TotalBlobTx = timelineMessages.Count(m => m.EventName.StartsWith("WAZStorage:Blob"));
                model.Statistics.TotalTableTx = timelineMessages.Count(m => m.EventName.StartsWith("WAZStorage:Table"));
                model.Statistics.TotalQueueTx = timelineMessages.Count(m => m.EventName.StartsWith("WAZStorage:Queue"));
                model.Statistics.TotalTrafficToStorage = timelineMessages.Sum(m => m.RequestSize).ToBytesHuman();
                model.Statistics.TotalTrafficFromStorage = timelineMessages.Sum(m => m.ResponseSize).ToBytesHuman();
                model.Statistics.PricePerTenThousandPageViews = string.Format("${0}", (model.Statistics.TotalStorageTx * 1000 * 0.0000001 + timelineMessages.Sum(m => m.ResponseSize) * 10000 * (0.12 / 1024 / 1024 / 1024)));

                //todo: inject inspections so they can be made extensible
                var inspectors = new IWindowsAzureStorageInspector[]
                {
                    new GeneralBestPracticesInspector(),
                    new TableStorageQueryIndexInspector()
                };
                foreach (var inspector in inspectors)
                {
                    var warnings = inspector.Inspect();
                    model.Warnings.AddRange(warnings.Select(m => new
                    {
                        RequestUrl = "General",
                        Warning = m
                    }));
                }

                foreach (var message in timelineMessages)
                {
                    foreach (var inspector in inspectors)
                    {
                        var warnings = inspector.Inspect(message);
                        model.Warnings.AddRange(warnings.Select(m => new
                        {
                            RequestUrl = message.Url,
                            Warning = m
                        }));
                    }
                }

                return model;
            }

            return "No storage transactions have been utilized for this request.";
        }
    }
}