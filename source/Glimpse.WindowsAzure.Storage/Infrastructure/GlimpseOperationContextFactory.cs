using System;
using System.Collections.Generic;
using Glimpse.Core.Extensibility;
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

                    MessageBroker.Publish(new WindowsAzureStorageTimelineMessage(serviceType, args.Request.Method, args.Request.RequestUri.ToString(), (int)args.Response.StatusCode, args.RequestInformation.StartTime.ToLocalTime(), point.StartTime, point.Offset, args));
                };
            return ctx;
        }

        private string DetermineServiceTypeFromUri(Uri requestUri)
        {
            if (requestUri.Host.Contains("blob") || requestUri.Port == 10000)
            {
                return "Blob";
            }

            if (requestUri.Host.Contains("queue") || requestUri.Port == 10001)
            {
                return "Queue";
            }

            if (requestUri.Host.Contains("table") || requestUri.Port == 10002)
            {
                return "Table";
            }

            return "(other)";
        }
    }
}