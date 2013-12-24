using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Message;

#if MVC2
using Glimpse.Mvc2.Backport;
#endif
#if MVC3
using Glimpse.Mvc3.Backport;
#endif

namespace Glimpse.Mvc.AlternateType
{
    public class ValueProvider<T> : AlternateType<T> where T : class
    {
        private IEnumerable<IAlternateMethod> allMethods;

        // This class manages alternate implementations of both IValueProvider, IUnvalidatedValueProvider and IEnumerableValueProvider
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
                        new ContainsPrefix(),
                        new GetKeysFromPrefix()
                    });
            }
        }

        public class ContainsPrefix : AlternateMethod
        {
            public ContainsPrefix() : base(typeof(IValueProvider), "ContainsPrefix")
            {
            }

            public override void PostImplementation(IAlternateMethodContext context, TimerResult timerResult)
            {
                var message = new Message((string)context.Arguments[0], (bool)context.ReturnValue, context.TargetType)
                    .AsSourceMessage(context.TargetType, context.MethodInvocationTarget);

                context.MessageBroker.Publish(message);
            }

            public class Message : MessageBase, ISourceMessage
            {
                public Message(string prefix, bool isMatch, Type valueProviderType)
                {
                    Prefix = prefix;
                    IsMatch = isMatch;
                    ValueProviderType = valueProviderType;
                }

                public bool IsMatch { get; private set; }

                public Type ValueProviderType { get; private set; }

                public string Prefix { get; private set; }

                public Type ExecutedType { get; set; }

                public MethodInfo ExecutedMethod { get; set; }
            }
        }

        public class GetValue : AlternateMethod
        {
            public GetValue() : base(typeof(IValueProvider), "GetValue")
            {
            }

            public override void PostImplementation(IAlternateMethodContext context, TimerResult timerResult)
            {
                var args = new Arguments(context.Arguments);
                var message = new Message(args.Key, context.ReturnValue as ValueProviderResult, context.TargetType)
                    .AsSourceMessage(context.TargetType, context.MethodInvocationTarget);

                context.MessageBroker.Publish(message);
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

            public class Message : MessageBase, ISourceMessage
            {
                public Message(string key, ValueProviderResult result, Type valueProviderType)
                {
                    Key = key;
                    IsFound = false;
                    if (result != null)
                    {
                        IsFound = true;
                        AttemptedValue = result.AttemptedValue;
                        Culture = CultureInfo.ReadOnly(result.Culture);
                        RawValue = result.RawValue;
                    }

                    ValueProviderType = valueProviderType;
                }

                public string Key { get; set; }

                public bool IsFound { get; private set; }

                public Type ValueProviderType { get; private set; }

                public object RawValue { get; private set; }

                public CultureInfo Culture { get; private set; }

                public string AttemptedValue { get; private set; }

                public Type ExecutedType { get; set; }

                public MethodInfo ExecutedMethod { get; set; }
            }
        }

        public class GetKeysFromPrefix : AlternateMethod
        {
            public GetKeysFromPrefix()
                : base(typeof(IEnumerableValueProvider), "GetKeysFromPrefix")
            {
            }

            public override void PostImplementation(IAlternateMethodContext context, TimerResult timerResult)
            {
            }
        }
    }
}