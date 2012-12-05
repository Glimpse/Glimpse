using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Message;

namespace Glimpse.Mvc.AlternateImplementation
{
    public class ValueProvider<T> : AlternateType<T> where T : class
    {
        private IEnumerable<IAlternateMethod> allMethods;

        // This class manages alternate implementations of both IValueProvider and IUnvalidatedValueProvider
        public ValueProvider(IProxyFactory proxyFactory) : base(proxyFactory)
        {
        }

        public override IEnumerable<IAlternateMethod> AllMethods
        {
            get
            {
                return allMethods ?? (allMethods = new List<IAlternateMethod>
                    {
                        new GetValue(), 
                        new ContainsPrefix()
                    });
            }
        }

        public class ContainsPrefix : AlternateMethod
        {
            public ContainsPrefix() : base(typeof(IValueProvider), "ContainsPrefix")
            {
            }

            public override void PostImplementation(IAlternateImplementationContext context, TimerResult timerResult)
            {
                context.MessageBroker.Publish(new Message((string)context.Arguments[0], (bool)context.ReturnValue, context.TargetType, context.MethodInvocationTarget));
            }

            public class Message : MessageBase
            {
                public Message(string prefix, bool isMatch, Type valueProviderType, MethodInfo executedMethod) : base(valueProviderType, executedMethod)
                {
                    Prefix = prefix;
                    IsMatch = isMatch;
                    ValueProviderType = valueProviderType;
                }

                public bool IsMatch { get; private set; }

                public Type ValueProviderType { get; private set; }

                public string Prefix { get; private set; }
            }
        }

        public class GetValue : AlternateMethod
        {
            public GetValue() : base(typeof(T), "GetValue")
            {
            }

            public override void PostImplementation(IAlternateImplementationContext context, TimerResult timerResult)
            {
                context.MessageBroker.Publish(new Message(new Arguments(context.Arguments), context.ReturnValue as ValueProviderResult, context.TargetType));
            }

            public class Arguments
            {
                public Arguments(params object[] arguments)
                {
                    SkipValidation = false;
                    Key = (string)arguments[0];

                    if (arguments.Length > 1)
                    {
                        SkipValidation = (bool)arguments[1];
                    }
                }

                public string Key { get; set; }

                public bool SkipValidation { get; set; }
            }

            public class Message : MessageBase
            {
                public Message(Arguments arguments, ValueProviderResult result, Type valueProviderType)
                {
                    IsFound = false;
                    if (result != null)
                    {
                        IsFound = true;
                        AttemptedValue = result.AttemptedValue;
                        Culture = result.Culture;
                        RawValue = result.RawValue;
                    }

                    ValueProviderType = valueProviderType;
                }

                public bool IsFound { get; set; }

                public Type ValueProviderType { get; set; }

                public object RawValue { get; set; }

                public CultureInfo Culture { get; set; }

                public string AttemptedValue { get; set; }
            }
        }
    }
}