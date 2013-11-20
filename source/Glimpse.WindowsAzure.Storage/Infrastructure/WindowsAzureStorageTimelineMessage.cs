using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using Glimpse.Core.Message;
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

            Url = resourceUri;

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

            EventSubText = string.Format("out: {0}/in: {1}", RequestSize.ToBytesHuman(), ResponseSize.ToBytesHuman());

            // todo: refactor, this has been written on a tight schedule
            if (serviceName == "Table" && Url.Contains("$filter="))
            {
                string filterString = HttpUtility.ParseQueryString(new Uri(Url).Query)["$filter"];

                var model = new EdmModel();
                var tableServiceEntity = new EdmEntityType("Glimpse.WindowsAzure.Storage", "TableServiceEntity");
                tableServiceEntity.AddStructuralProperty("PartitionKey", EdmPrimitiveTypeKind.String);
                tableServiceEntity.AddStructuralProperty("RowKey", EdmPrimitiveTypeKind.String);
                tableServiceEntity.AddStructuralProperty("Timestamp", EdmPrimitiveTypeKind.DateTime);
                model.AddElement(tableServiceEntity);

                var container= new EdmEntityContainer("TestModel", "DefaultContainer");
                container.AddEntitySet("Entities", tableServiceEntity); 
                model.AddElement(container);

                try
                {
                    ODataUriParser.ParseFilter(filterString, model, tableServiceEntity);
                }
                catch (ODataException ex)
                {
                    // this means we're using other properties...
                    Warning = "This query may not perform as intended. Consider optimizing the entity structure so that a PartitionKey/RowQuery can be executed instead.";
                }
            }
        }

        private class ODataSingleValuePropertyAccessNodeSearcher
            : QueryNodeVisitor<SingleValuePropertyAccessNode>
        {
            private List<SingleValuePropertyAccessNode> nodes;

            public ODataSingleValuePropertyAccessNodeSearcher()
            {
                nodes = new List<SingleValuePropertyAccessNode>();
            }

            public List<SingleValuePropertyAccessNode> Parse(SingleValueNode startingNode)
            {
                return nodes;
            }

            public override SingleValuePropertyAccessNode Visit(SingleValuePropertyAccessNode nodeIn)
            {
                nodes.Add(nodeIn);
                return base.Visit(nodeIn);
            }
        }

        public Guid Id { get; private set; }
        public string Url { get; set; }
        public Type ExecutedType { get; set; }
        public MethodInfo ExecutedMethod { get; set; }
        public TimeSpan Offset { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime StartTime { get; set; }
        public string EventName { get; set; }
        public TimelineCategoryItem EventCategory { get; set; }
        public string EventSubText { get; set; }

        public long RequestSize { get; set; }
        public long ResponseSize { get; set; }
        public string Warning { get; set; }
    }
}