using System;
using System.Collections.Generic;
using System.Web;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library;
using Microsoft.Data.OData;
using Microsoft.Data.OData.Query;
using Microsoft.WindowsAzure.Storage;

namespace Glimpse.WindowsAzure.Storage.Infrastructure.Inspections
{
    public class TableStorageEchoContentInspector
        : WindowsAzureStorageInspectorBase
    {
        public override IEnumerable<string> Inspect(WindowsAzureStorageTimelineMessage message)
        {
            if (message.ServiceName == "Table" &&
                (message.ServiceOperation == "PUT" || message.ServiceOperation == "POST" || message.ServiceOperation == "MERGE" || message.ServiceOperation == "PATCH"))
            {
                var preferHeader = message.RequestHeaders["Prefer"];
                var dataServiceVersionHeader = message.RequestHeaders["DataServiceVersion"];
                if (preferHeader != null && dataServiceVersionHeader != null && dataServiceVersionHeader.Contains("3.0") && preferHeader.ToLowerInvariant() != "return-no-content")
                {
                    // We're on Storage SDK 3.0 and it's an insert/update/merge
                    return new string[] { "Disable the echocontent feature on the TableOperation instance or set the HTTP header Prefer:return-no-content. This will ensure the message payload isn't unnecessarily being returned in the operation response." };
                }
            }
            return new string[] { };
        }
    }
}