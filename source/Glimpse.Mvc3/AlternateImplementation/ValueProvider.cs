using System.Collections.Generic;
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
            yield return new GetValue<T>();
        }

        public class GetValue<T> : IAlternateImplementation<T> where T : class
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

                context.MessageBroker.Publish(new Message(new Arguments(context.Arguments)));
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
                public Message(Arguments arguments)
                {
                    // TODO: Add meaningful properties to message w/ test
                }
            }
        }
    }
}