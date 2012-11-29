using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;

namespace Glimpse.Mvc.AlternateImplementation
{
    public class ValueProvider<T> : Alternate<T> where T : class
    {
        // This class manages alternate implementations of both IValueProvider and IUnvalidatedValueProvider
        public ValueProvider(IProxyFactory proxyFactory) : base(proxyFactory)
        {
        }

        public override IEnumerable<IAlternateMethod> AllMethods()
        {
            yield return new GetValue();
            yield return new ContainsPrefix();
        }

        public class ContainsPrefix : IAlternateMethod
        {
            public ContainsPrefix()
            {
                MethodToImplement = typeof(IValueProvider).GetMethod("ContainsPrefix");
            }

            public MethodInfo MethodToImplement { get; private set; }

            public void NewImplementation(IAlternateImplementationContext context)
            {
                TimerResult timerResult;
                if (!context.TryProceedWithTimer(out timerResult))
                {
                    return;
                }

                context.MessageBroker.Publish(new Message((string)context.Arguments[0], (bool)context.ReturnValue, context.TargetType));
            }

            public class Message : MessageBase
            {
                public Message(string prefix, bool isMatch, Type valueProviderType)
                {
                    Prefix = prefix;
                    IsMatch = isMatch;
                    ValueProviderType = valueProviderType;
                }

                public bool IsMatch { get; set; }

                public Type ValueProviderType { get; set; }

                public string Prefix { get; set; }
            }
        }

        public class GetValue : IAlternateMethod
        {
            public GetValue()
            {
                MethodToImplement = typeof(T).GetMethod("GetValue");
            }

            public MethodInfo MethodToImplement { get; private set; }

            public void NewImplementation(IAlternateImplementationContext context)
            {
                TimerResult timerResult;
                if (!context.TryProceedWithTimer(out timerResult))
                {
                    return;
                }

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