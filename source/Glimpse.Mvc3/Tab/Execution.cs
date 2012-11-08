using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Async;
using Glimpse.AspNet.Extensibility;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Mvc.Message;

namespace Glimpse.Mvc.Tab
{
    public class Execution : AspNetTab, ITabSetup
    {
        private const string TabStoreKey = "IActionFilterMessageKey";

        public override string Name
        {
            get { return "Execution"; }
        }

        public override object GetData(ITabContext context)
        {
            var actionFilterMessages = context.TabStore.Get<IList<IActionFilterMessage>>(TabStoreKey);
            return actionFilterMessages;
        }

        public void Setup(ITabSetupContext context)
        {
            var messageBroker = context.MessageBroker;
                
            messageBroker.Subscribe<IActionFilterMessage>(message => PersistActionFilterMessage(message, context));



            messageBroker.Subscribe<ActionInvoker.InvokeActionMethod.Message>(message => Persist(message, context)); // check
            messageBroker.Subscribe<ActionInvoker.InvokeActionResult<ControllerActionInvoker>.Message>(message => Persist(message, context)); // check
            messageBroker.Subscribe<ActionInvoker.InvokeActionResult<AsyncControllerActionInvoker>.Message>(message => Persist(message, context)); // check

            // messageBroker.Subscribe<ActionFilter.OnActionExecuted.Message>(message => Persist(message, context)); // check
            // messageBroker.Subscribe<ActionFilter.OnActionExecuting.Message>(message => Persist(message, context)); // check
            // messageBroker.Subscribe<AuthorizationFilter.OnAuthorization.Message>(message => Persist(message, context)); // check
            // messageBroker.Subscribe<ExceptionFilter.OnException.Message>(message => Persist(message, context)); // check
            // messageBroker.Subscribe<ResultFilter.OnResultExecuted.Message>(message => Persist(message, context)); // check
            // messageBroker.Subscribe<ResultFilter.OnResultExecuting.Message>(message => Persist(message, context)); // check
        }

        internal static void PersistActionFilterMessage(IActionFilterMessage message, ITabSetupContext context)
        {
            var tabStore = context.GetTabStore();

            if (!tabStore.Contains(TabStoreKey))
            {
                tabStore.Set(TabStoreKey, new List<IActionFilterMessage>());
            }

            var messages = tabStore.Get<IList<IActionFilterMessage>>(TabStoreKey);

            messages.Add(message);
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