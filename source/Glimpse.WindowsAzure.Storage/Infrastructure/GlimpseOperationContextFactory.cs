using System;
using System.Collections.Generic;
using System.Reflection;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Message;
using Microsoft.WindowsAzure.Storage;

namespace Glimpse.WindowsAzure.Storage.Infrastructure
{
    public class GlimpseOperationContextFactory
        : IOperationContextFactory
    {
        protected const string Category = "ASP.NET";

        public GlimpseOperationContextFactory(IMessageBroker messageBroker, Func<IExecutionTimer> timerStrategy)
        {
            MessageBroker = messageBroker;
            TimerStrategy = timerStrategy;
        }

        public IMessageBroker MessageBroker { get; set; }
        public Func<IExecutionTimer> TimerStrategy { get; set; }

        public OperationContext Create()
        {
            var ctx = new OperationContext();
            ctx.ResponseReceived += (sender, args) =>
                {
                    var serviceType = DetermineServiceTypeFromUri(args.Request.RequestUri);
                    var point = TimerStrategy().Point();

                    MessageBroker.Publish(new Message(serviceType, args.Request.Method, args.Request.RequestUri.ToString(), (int)args.Response.StatusCode, args.RequestInformation.StartTime.ToLocalTime(), point.StartTime, point.Offset, args));
                };
            return ctx;
        }

        private string DetermineServiceTypeFromUri(Uri requestUri)
        {
            if (requestUri.Host.Contains("blob") || requestUri.Port == 10000)
            {
                return "Blob";
            }
            if (requestUri.Host.Contains("table") || requestUri.Port == 10001)
            {
                return "Table";
            }
            if (requestUri.Host.Contains("queue") || requestUri.Port == 10002)
            {
                return "Queue";
            } 
            return "(other)";
        }

        public class Message 
            : ITimelineMessage
        {
            public Message(string serviceName, string serviceOperation, string resourceUri, int responseCode, DateTime startTime, DateTime endTime, TimeSpan offset, RequestEventArgs requestEventArgs)
            {
                Id = Guid.NewGuid();

                EventName = string.Format("WAZStorage:{0} - {1} {2} {3}", serviceName, serviceOperation, resourceUri, responseCode);
                EventCategory = new TimelineCategoryItem("Windows Azure Storage", "#0094FF", "#0094FF");

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
            }

            public Guid Id { get; private set; }
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

            public void BuildDetails(IDictionary<string, object> details)
            {
                // requestEventArgs contains a lot of interesting stuff...
                details.Add("RequestSize", RequestSize);
                details.Add("ResponseSize", ResponseSize);
            }
        }
    }
}