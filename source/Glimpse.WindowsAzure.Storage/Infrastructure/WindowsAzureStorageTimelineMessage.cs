using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using Glimpse.Core.Message;
using Glimpse.WindowsAzure.Storage.Infrastructure.Inspections;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library;
using Microsoft.Data.OData;
using Microsoft.Data.OData.Query;
using Microsoft.Data.OData.Query.SemanticAst;
using Microsoft.WindowsAzure.Storage;

namespace Glimpse.WindowsAzure.Storage.Infrastructure
{
    public class WindowsAzureStorageTimelineMessage
        : ITimelineMessage
    {
        public WindowsAzureStorageTimelineMessage(string serviceName, string serviceOperation, string resourceUri, int responseCode, DateTime startTime, DateTime endTime, TimeSpan offset, RequestEventArgs requestEventArgs)
        {
            Id = Guid.NewGuid();

            EventName = string.Format("WAZStorage:{0} - {1} {2} {3}", serviceName, serviceOperation, resourceUri, responseCode);
            EventCategory = new TimelineCategoryItem("Windows Azure Storage", "#0094FF", "#0094FF");

            ServiceName = serviceName;
            ServiceOperation = serviceOperation;
            Url = resourceUri;
            ResponseCode = responseCode;

            StartTime = startTime;
            Duration = endTime - startTime;
            Offset = offset;

            RequestSize = requestEventArgs.Request.ContentLength;
            foreach (var header in requestEventArgs.Request.Headers.AllKeys)
            {
                RequestSize += header.Length + 2 + string.Join(";", requestEventArgs.Request.Headers.GetValues(header)).Length;
            }
            ResponseSize = requestEventArgs.Response.ContentLength;
            foreach (var header in requestEventArgs.Response.Headers.AllKeys)
            {
                ResponseSize += header.Length + 2 + string.Join(";", requestEventArgs.Response.Headers.GetValues(header)).Length;
            }

            RequestHeaders = requestEventArgs.Request.Headers;
            ResponseHeaders = requestEventArgs.Request.Headers;

            EventSubText = string.Format("out: {0}/in: {1}", RequestSize.ToBytesHuman(), ResponseSize.ToBytesHuman());
        }

        public Guid Id { get; private set; }
        public string Url { get; set; }
        public int ResponseCode { get; set; }
        public TimeSpan Offset { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime StartTime { get; set; }
        public string EventName { get; set; }
        public TimelineCategoryItem EventCategory { get; set; }
        public string EventSubText { get; set; }
        public string ServiceOperation { get; set; }
        public string ServiceName { get; set; }
        public long RequestSize { get; set; }
        public long ResponseSize { get; set; }
        public WebHeaderCollection ResponseHeaders { get; set; }
        public WebHeaderCollection RequestHeaders { get; set; }
    }
}