using System;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Core.Message;

namespace Glimpse.Core
{
    public static class GlimpseTimeline
    {
        public static OngoingCapture Capture(string eventName)
        {
            return Capture(eventName, string.Empty);
        }

        public static OngoingCapture Capture(string eventName, string eventSubText)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                throw new ArgumentNullException("eventName");
            }

            if (!GlimpseRuntime.IsInitialized)
            {
                return OngoingCapture.Empty();
            }

            var messageBroker = GlimpseRuntime.Instance.Configuration.MessageBroker;
            var executionTimer = GlimpseRuntime.Instance.CurrentRequestContext.CurrentExecutionTimer;

            return new OngoingCapture(executionTimer, messageBroker, eventName, eventSubText);
        }

        public static void CaptureMoment(string eventName)
        {
            CaptureMoment(eventName, string.Empty);
        }

        public static void CaptureMoment(string eventName, string eventSubText)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                throw new ArgumentNullException("eventName");
            }

            if (!GlimpseRuntime.IsInitialized)
            {
                return;
            }

            var messageBroker = GlimpseRuntime.Instance.Configuration.MessageBroker;
            var executionTimer = GlimpseRuntime.Instance.CurrentRequestContext.CurrentExecutionTimer;

            messageBroker.Publish(new TimelineMessage(executionTimer.Point(), eventName, eventSubText));
        }

        public class OngoingCapture : IDisposable
        {
            public static OngoingCapture Empty()
            {
                return new NullOngoingCapture();
            }

            private OngoingCapture()
            {
            }

            public OngoingCapture(IExecutionTimer executionTimer, IMessageBroker messageBroker, string eventName, string eventSubText)
            {
                Offset = executionTimer.Start();
                ExecutionTimer = executionTimer;
                EventSubText = eventSubText;
                MessageBroker = messageBroker;
                EventName = eventName;
            }

            private string EventSubText { get; set; }

            private string EventName { get; set; }

            private TimeSpan Offset { get; set; }

            private IExecutionTimer ExecutionTimer { get; set; }

            private IMessageBroker MessageBroker { get; set; }

            public virtual void Stop()
            {
                var timerResult = ExecutionTimer.Stop(Offset);
                  
                MessageBroker.Publish(new TimelineMessage(timerResult, EventName, EventSubText));
            }

            public void Dispose()
            {
                Stop();
            }

            private class NullOngoingCapture : OngoingCapture
            {
                public override void Stop()
                {
                }
            }
        }
    }
}