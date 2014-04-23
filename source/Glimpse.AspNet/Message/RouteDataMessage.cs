using System;
using System.Reflection;
using Glimpse.Core.Message;

namespace Glimpse.AspNet.Message
{
    public class RouteDataMessage : MessageBase, ITimedMessage, ISourceMessage
    {
        public RouteDataMessage(int routeHashCode, System.Web.Routing.RouteData routeData, string routeName)
        {
            IsMatch = routeData != null;
            RouteHashCode = routeHashCode;
            RouteName = routeName;

            if (routeData != null)
            {
                Values = routeData.Values;
            }
        }

        public TimeSpan Offset { get; set; }

        public TimeSpan Duration { get; set; }

        public DateTime StartTime { get; set; }

        public Type ExecutedType { get; set; }

        public MethodInfo ExecutedMethod { get; set; }

        public System.Web.Routing.RouteValueDictionary Values { get; protected set; }

        public int RouteHashCode { get; protected set; }

        public bool IsMatch { get; protected set; }

        public string RouteName { get; protected set; }
    }
}
