using System.Linq;
using Glimpse.AspNet.Extensibility;
using Glimpse.Core.Extensibility;
using System.Collections.Generic;
using Glimpse.Mvc3.AlternateImplementation;
using Glimpse.Mvc3.Model;

namespace Glimpse.Mvc3.Tab
{
    public class Views : AspNetTab, ITabSetup
    {
        public override object GetData(ITabContext context)
        {
            var tabStore = context.TabStore;
            var viewEngineFindViewsMessages = tabStore.Get<List<ViewEngine.FindViews.Message>>(typeof(ViewEngine.FindViews.Message).FullName);
            var viewRenderMessages = tabStore.Get<List<View.Render.Message>>(typeof(View.Render.Message).FullName);
            var result = new List<ViewsModel>();

            if (viewEngineFindViewsMessages == null || viewRenderMessages == null)
                return result;

            foreach (var findView in viewEngineFindViewsMessages)
            {
                result.Add(new ViewsModel(findView, viewRenderMessages.SingleOrDefault(r => r.Mixin.ViewEngineFindCallId == findView.Id)));
            }

            return result;
        }

        public override string Name
        {
            get { return "Views"; }
        }

        public void Setup(ITabSetupContext context)
        {
            var messageBroker = context.MessageBroker;

            messageBroker.Subscribe<ViewEngine.FindViews.Message>(message => Persist(message, context));
            messageBroker.Subscribe<View.Render.Message>(message => Persist(message, context));
        }

        internal static void Persist<T>(T message, ITabSetupContext context)
        {
            var tabStore = context.GetTabStore();
            var key = typeof(T).FullName;

            if (!tabStore.Contains(key))
                tabStore.Set(key, new List<T>());

            var messages = tabStore.Get<IList<T>>(key);

            messages.Add(message);
        }
    }
}