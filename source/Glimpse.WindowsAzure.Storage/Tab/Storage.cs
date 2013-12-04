using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls.WebParts;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Message;
using Glimpse.Core.Tab.Assist;
using Glimpse.WindowsAzure.Storage.Infrastructure;
using Glimpse.WindowsAzure.Storage.Infrastructure.Inspections;
using Glimpse.WindowsAzure.Storage.Models;

namespace Glimpse.WindowsAzure.Storage.Tab
{
    public class Storage : TabBase, IKey, ITabSetup, ILayoutControl, ITabLayout
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
        public object GetLayout()
        {
            return Layout;
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

                model.Requests = FlattenRequests(timelineMessages);
                model.Warnings = AnalyzeMessagesForWarnings(timelineMessages);

                return model;
            }

            return "No storage transactions have been utilized for this request.";
        }

        private List<StorageRequestModel> FlattenRequests(IEnumerable<WindowsAzureStorageTimelineMessage> timelineMessages)
        {
            return timelineMessages.Select(m => new StorageRequestModel
            {
                Service = m.ServiceName,
                Operation = m.ServiceOperation,
                Url = m.Url,
                ResponseCode = m.ResponseCode,
                ResponseSize = m.ResponseSize,
                Duration = m.Duration,
                Offset = m.Offset
            }).ToList();
        }

        private List<StorageWarningModel> AnalyzeMessagesForWarnings(IEnumerable<WindowsAzureStorageTimelineMessage> timelineMessages)
        {
            List<StorageWarningModel> returnValue = new List<StorageWarningModel>();

            //todo: inject inspections so they can be made extensible
            var inspectors = new IWindowsAzureStorageInspector[]
                {
                    new GeneralBestPracticesInspector(),
                    new TableStorageQueryIndexInspector(),
                    new TableStorageEchoContentInspector()
                };
            foreach (var inspector in inspectors)
            {
                var warnings = inspector.Inspect();
                returnValue.AddRange(warnings.Select(m => new StorageWarningModel
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
                    returnValue.AddRange(warnings.Select(m => new StorageWarningModel
                    {
                        RequestUrl = message.Url,
                        Warning = m
                    }));
                }
            }

            return returnValue;
        }

        private static readonly object Layout =
            TabLayout.Create()
                .Cell("requests", TabLayout.Create().Row(r =>
                {
                    r.Cell("service").WidthInPixels(100);
                    r.Cell("operation").WidthInPixels(80);
                    r.Cell("responseCode").WidthInPixels(120);
                    r.Cell("responseSize").WidthInPixels(120);
                    r.Cell("url");
                    r.Cell("duration").WidthInPixels(150).Suffix(" ms").Class("mono").AlignRight();
                    r.Cell("offset").WidthInPixels(150).Prefix("T+ ").Suffix(" ms").Class("mono").AlignRight();
                })) 
                .Build();
    }
}