using System;
using System.Collections.Generic;
using System.Reflection;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Message;

namespace Glimpse.AspNet.Message
{ 
    public class ProcessConstraintMessage : TimeMessage
    {
        public ProcessConstraintMessage(TimerResult timer, Type executedType, MethodInfo executedMethod, bool isMatch, string parameterName, object constraint, IDictionary<string, object> values)
            : base(timer)
        {
            ExecutedMethod = executedMethod;
            ExecutedType = executedType;
            IsMatch = isMatch;
            ParameterName = parameterName;
            Constraint = constraint;
            Values = values;
        }

        public MethodInfo ExecutedMethod { get; private set; }

        public Type ExecutedType { get; private set; }

        public bool IsMatch { get; private set; }

        public IDictionary<string, object> Values { get; private set; }

        public string ParameterName { get; private set; }

        public object Constraint { get; private set; }
    } 
}
