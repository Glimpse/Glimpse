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
            return Capture(eventName, null, TimelineCategory.User, new TimelineMessage());
        }

        public static OngoingCapture Capture(string eventName, string eventSubText)
        {
            return Capture(eventName, eventSubText, TimelineCategory.User, new TimelineMessage());
        }

        internal static OngoingCapture Capture(string eventName, TimelineCategoryItem category)
        {
            return Capture(eventName, null, category, new TimelineMessage());
        }

        internal static OngoingCapture Capture(string eventName, TimelineCategoryItem category, ITimelineMessage message)
        {
            return Capture(eventName, null, category, message);
        }

        internal static OngoingCapture Capture(string eventName, ITimelineMessage message)
        {
            return Capture(eventName, null, TimelineCategory.User, message);
        }

        internal static OngoingCapture Capture(string eventName, string eventSubText, TimelineCategoryItem category, ITimelineMessage message)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                throw new ArgumentNullException("eventName");
            }

            if (!GlimpseRuntime.IsAvailable)
            {
                return OngoingCapture.Empty();
            }

            var messageBroker = GlimpseRuntime.Instance.Configuration.MessageBroker;
            var executionTimer = GlimpseRuntime.Instance.CurrentRequestContext.CurrentExecutionTimer;

            return new OngoingCapture(executionTimer, messageBroker, eventName, eventSubText, category, message);
        }

        public static void CaptureMoment(string eventName)
        {
            CaptureMoment(eventName, null, TimelineCategory.User, new TimelineMessage());
        }

        public static void CaptureMoment(string eventName, string eventSubText)
        {
            CaptureMoment(eventName, eventSubText, TimelineCategory.User, new TimelineMessage());
        }

        internal static void CaptureMoment(string eventName, TimelineCategoryItem category)
        {
            CaptureMoment(eventName, null, category, new TimelineMessage());
        }

        internal static void CaptureMoment(string eventName, TimelineCategoryItem category, ITimelineMessage message) 
        {
            CaptureMoment(eventName, null, category, message);
        }

        internal static void CaptureMoment(string eventName, ITimelineMessage message) 
        {
            CaptureMoment(eventName, null, TimelineCategory.User, message);
        }

        internal static void CaptureMoment(string eventName, string eventSubText, TimelineCategoryItem category, ITimelineMessage message) 
        {
            if (string.IsNullOrEmpty(eventName))
            {
                throw new ArgumentNullException("eventName");
            }

            if (!GlimpseRuntime.IsAvailable)
            {
                return;
            }

            var messageBroker = GlimpseRuntime.Instance.Configuration.MessageBroker;
            var executionTimer = GlimpseRuntime.Instance.CurrentRequestContext.CurrentExecutionTimer;

            message
                .AsTimelineMessage(eventName, category, eventSubText)
                .AsTimedMessage(executionTimer.Point());

            messageBroker.Publish(message);
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

            public OngoingCapture(IExecutionTimer executionTimer, IMessageBroker messageBroker, string eventName, string eventSubText, TimelineCategoryItem category, ITimelineMessage message)
            {
                Offset = executionTimer.Start();
                ExecutionTimer = executionTimer;
                Message = message.AsTimelineMessage(eventName, category, eventSubText);  
                MessageBroker = messageBroker;  
            }

            private ITimelineMessage Message { get; set; } 

            private TimeSpan Offset { get; set; }

            private IExecutionTimer ExecutionTimer { get; set; }

            private IMessageBroker MessageBroker { get; set; }

            public virtual void Stop()
            {
                var timerResult = ExecutionTimer.Stop(Offset);

                MessageBroker.Publish(Message.AsTimedMessage(timerResult));
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