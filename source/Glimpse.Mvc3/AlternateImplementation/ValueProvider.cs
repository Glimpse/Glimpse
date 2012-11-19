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

        public override IEnumerable<IAlternateImplementation<T>> AllMethods()
        {
            yield return new GetValue();
        }

        public class GetValue : IAlternateImplementation<T>
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
                    AttemptedValue = result.AttemptedValue;
                    Culture = result.Culture;
                    RawValue = result.RawValue;
                    ValueProviderType = valueProviderType;
                }

                public Type ValueProviderType { get; set; }

                public object RawValue { get; set; }

                public CultureInfo Culture { get; set; }

                public string AttemptedValue { get; set; }
            }
        }
    }
}