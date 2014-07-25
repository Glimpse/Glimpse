using Microsoft.AspNet.Builder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Glimpse.ProjectK.Middleware
{
    public class MiddlewareExecutionInfo
    {
        private string title;
        private Stopwatch stopwatch;
        private TimeSpan? childlessDuration;

        public static MiddlewareExecutionInfo Unrun(Func<RequestDelegate, RequestDelegate> type)
        {
            return new MiddlewareExecutionInfo {Type = type};
        }

        public static MiddlewareExecutionInfo Running(Func<RequestDelegate, RequestDelegate> type)
        {
            return new MiddlewareExecutionInfo
            {
                Type = type,
                stopwatch = Stopwatch.StartNew(),
            };
        }

        public MiddlewareExecutionInfo()
        {
            Children = new List<MiddlewareExecutionInfo>();
        }

        public void Stop()
        {
            stopwatch.Stop();
        }

        public Func<RequestDelegate, RequestDelegate> Type { get; set; }

        public TimeSpan? Duration 
        {
            get
            {
                if (stopwatch == null)
                    return null;

                return stopwatch.Elapsed;
            }
        }

        public TimeSpan? ChildlessDuration 
        {
            get
            {
                if (childlessDuration.HasValue)
                    return childlessDuration.Value;

                if (!Duration.HasValue)
                    return null;

                var duration = Duration.Value;
                foreach (var child in Children)
                {
                    duration -= child.ChildlessDuration.HasValue ? child.ChildlessDuration.Value : TimeSpan.Zero;
                }

                childlessDuration = duration;
                return duration;
            }
        }

        public string Title 
        {
            get
            {
                // This is the only remotely relevant piece of information I could find left in Type
                return title ?? (title = Regex.Replace(Type.Method.Module.Name, "(?<=[a-z])([A-Z])", " $1")
                    .Replace(" Middleware", string.Empty));
            }
        }

        public ICollection<MiddlewareExecutionInfo> Children { get; set; }
    }
}