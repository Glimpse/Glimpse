using System;
using System.Reflection;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Message;
using Microsoft.ApplicationServer.Caching;

namespace Glimpse.WindowsAzure.Caching.Infrastructure
{
    public class GlimpseDataCacheEventSubscriber
        : IDataCacheEventSubscriber
    {
        protected const string Category = "ASP.NET";

        public GlimpseDataCacheEventSubscriber(IMessageBroker messageBroker, Func<IExecutionTimer> timerStrategy)
        {
            MessageBroker = messageBroker;
            TimerStrategy = timerStrategy;
        }

        public IMessageBroker MessageBroker { get; set; }

        public Func<IExecutionTimer> TimerStrategy { get; set; }

        // TODO: unsubscribe when Glimpse goes out of context
        public void Subscribe(DataCache cache)
        {
            cache.CacheOperationStarted += CacheOnCacheOperationStarted;
            cache.CacheOperationCompleted += CacheOnCacheOperationCompleted;
        }

        public void Unsubscribe(DataCache cache)
        {
            cache.CacheOperationStarted -= CacheOnCacheOperationStarted;
            cache.CacheOperationCompleted -= CacheOnCacheOperationCompleted;
        }

        // TODO: build useful data from this
        private void CacheOnCacheOperationStarted(object sender, CacheOperationStartedEventArgs args)
        {
            var cacheName = "default";
            var cache = sender as DataCache;
            if (cache != null)
            {
                cacheName = cache.Name;
            }

            MessageBroker.Publish(new Message(cacheName, args.OperationType.ToString()));
        }

        // TODO: build useful data from this
        private void CacheOnCacheOperationCompleted(object sender, CacheOperationCompletedEventArgs args)
        {
            // MessageBroker.Publish(new Message(serviceType, args.Request.Method, args.Request.RequestUri.ToString(), (int)args.Response.StatusCode, args.RequestInformation.StartTime.ToLocalTime(), point.StartTime, point.Offset, args));
        }

        public class Message : ITimelineMessage
        {
            public Message(string cacheName, string operationType) // , DateTime startTime, DateTime endTime, TimeSpan offset)
            {
                Id = Guid.NewGuid();

                EventName = string.Format("WAZCache:{0}:{1}", cacheName, operationType);
                EventCategory = new TimelineCategoryItem("Windows Azure Caching", "#0094FF", "#0094FF");

                // StartTime = startTime;
                // Duration = endTime - startTime;
                // Offset = offset;

                // RequestSize = requestEventArgs.Request.ContentLength;
                // foreach (var header in requestEventArgs.Request.Headers.AllKeys)
                // {
                //     RequestSize += header.Length + 2 + string.Join(";", requestEventArgs.Request.Headers.GetValues(header)).Length;
                // }
                // ResponseSize = requestEventArgs.Response.ContentLength;
                // foreach (var header in requestEventArgs.Response.Headers.AllKeys)
                // {
                //     ResponseSize += header.Length + 2 + string.Join(";", requestEventArgs.Response.Headers.GetValues(header)).Length;
                // }

                // EventSubText = string.Format("out: {0}/in: {1}", RequestSize.ToBytesHuman(), ResponseSize.ToBytesHuman());
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
        }
    }
}