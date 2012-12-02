using System;
using System.Collections.Generic;
using System.Reflection;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Message;

namespace Glimpse.AspNet.Message
{ 
    public class ProcessConstraintMessage : TimeMessage
    {
        public ProcessConstraintMessage(TimerResult timer, Type executedType, MethodInfo executedMethod, int routeHashCode, bool isMatch, string parameterName, object constraint, IDictionary<string, object> values)
            : base(timer, executedType, executedMethod)
        { 
            RouteHashCode = routeHashCode;
            IsMatch = isMatch;
            ParameterName = parameterName;
            Constraint = constraint;
            Values = values;
        }
         
        public int RouteHashCode { get; private set; }

        public bool IsMatch { get; private set; }

        public IDictionary<string, object> Values { get; private set; }

        public string ParameterName { get; private set; }

        public object Constraint { get; private set; }
    } 
}
