using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Async;
using Glimpse.AspNet.Extensibility;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateImplementation;

namespace Glimpse.Mvc.Tab
{
    public class Execution : AspNetTab, ITabSetup
    {
        public override string Name
        {
            get { return "Execution"; }
        }

        public override object GetData(ITabContext context)
        {
            return null;
            //// throw new NotImplementedException();
        }

        public void Setup(ITabSetupContext context)
        {
            var messageBroker = context.MessageBroker;

            messageBroker.Subscribe<ActionFilter.OnActionExecuted.Message>(message => Persist(message, context)); // check
            messageBroker.Subscribe<ActionFilter.OnActionExecuting.Message>(message => Persist(message, context)); // check

            messageBroker.Subscribe<ActionInvoker.InvokeActionMethod.Message>(message => Persist(message, context));
            messageBroker.Subscribe<ActionInvoker.InvokeActionResult<ControllerActionInvoker>.Message>(message => Persist(message, context));
            messageBroker.Subscribe<ActionInvoker.InvokeActionResult<AsyncControllerActionInvoker>.Message>(message => Persist(message, context));

            messageBroker.Subscribe<AuthorizationFilter.OnAuthorization.Message>(message => Persist(message, context)); // check
            messageBroker.Subscribe<ExceptionFilter.OnException.Message>(message => Persist(message, context)); // check
            messageBroker.Subscribe<ResultFilter.OnResultExecuted.Message>(message => Persist(message, context)); // check
            messageBroker.Subscribe<ResultFilter.OnResultExecuting.Message>(message => Persist(message, context)); // check
        }

        internal static void Persist<T>(T message, ITabSetupContext context)
        {
            var tabStore = context.GetTabStore();
            var key = typeof(T).FullName;

            if (!tabStore.Contains(key))
            {
                tabStore.Set(key, new List<T>());
            }

            var messages = tabStore.Get<IList<T>>(key);

            messages.Add(message);
        }
    }
}