using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Glimpse.Owin.Middleware
{
    public class MiddlewareExecutionInfo
    {
        private string title;
        private Stopwatch stopwatch;
        private TimeSpan? childlessDuration;

        public static MiddlewareExecutionInfo Unrun(Type type)
        {
            return new MiddlewareExecutionInfo {Type = type};
        }

        public static MiddlewareExecutionInfo Running(Type type)
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

        public Type Type { get; set; }

        public TimeSpan Duration 
        {
            get { return stopwatch.Elapsed; }
        }

        public TimeSpan ChildlessDuration 
        {
            get
            {
                if (childlessDuration.HasValue)
                    return childlessDuration.Value;

                var duration = Duration;
                foreach (var child in Children)
                {
                    duration -= child.ChildlessDuration;
                }

                childlessDuration = duration;
                return duration;
            }
        }

        public string Title 
        {
            get
            {
                return title ?? (title = Regex.Replace(Type.Name, "(?<=[a-z])([A-Z])", " $1")
                    .Replace(" Middleware", string.Empty));
            }
        }

        public ICollection<MiddlewareExecutionInfo> Children { get; set; }
    }
}