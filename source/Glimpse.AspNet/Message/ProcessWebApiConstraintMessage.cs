using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http.Routing;
using Glimpse.Core.Message;

namespace Glimpse.AspNet.Message
{
    public class ProcessWebApiConstraintMessage : ITimedMessage, ISourceMessage
    {
        public ProcessWebApiConstraintMessage(int routeHashCode, int constraintHashCode, bool isMatch, string parameterName, object constraint, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            RouteHashCode = routeHashCode;
            ConstraintHashCode = constraintHashCode;
            IsMatch = isMatch;
            ParameterName = parameterName;
            Constraint = constraint;
            Values = values;
            RouteDirection = routeDirection;
        }

        public Guid Id { get; private set; }

        public TimeSpan Offset { get; set; }

        public TimeSpan Duration { get; set; }

        public DateTime StartTime { get; set; }

        public Type ExecutedType { get; set; }

        public MethodInfo ExecutedMethod { get; set; }

        public int RouteHashCode { get; private set; }

        public int ConstraintHashCode { get; private set; }

        public bool IsMatch { get; private set; }

        public IDictionary<string, object> Values { get; private set; }

        public string ParameterName { get; private set; }

        public object Constraint { get; private set; }

        public HttpRouteDirection RouteDirection { get; private set; }
    }
}
