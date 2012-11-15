using System.Collections.Generic;
using System.Linq;
using Glimpse.AspNet.Extensibility;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Mvc.Model;

namespace Glimpse.Mvc.Tab
{
    using Glimpse.Core.Plugin.Assist;

    public class Views : AspNetTab, ITabSetup, ITabLayout
    {
        private static readonly object Layout = TabLayout.Create()
                .Row(r =>
                {
                    r.Cell(0).AsKey().WidthInPixels(60);
                    r.Cell(1).WidthInPixels(160);
                    r.Cell(2).WidthInPixels(160);
                    r.Cell(3);
                    r.Cell(4).WidthInPixels(60);
                    r.Cell(5).WidthInPercent(15);
                    r.Cell(6).WidthInPixels(80);
                    r.Cell(7).WidthInPixels(60);
                    r.Cell(8);
                }).Build();

        public override string Name
        {
            get { return "Views"; }
        }

        public object GetLayout()
        {
            return Layout;
        }

        public override object GetData(ITabContext context)
        {
            var tabStore = context.TabStore;
            var viewEngineFindViewsMessages = tabStore.Get<List<ViewEngine.FindViews.Message>>(typeof(ViewEngine.FindViews.Message).FullName);
            var viewRenderMessages = tabStore.Get<List<View.Render.Message>>(typeof(View.Render.Message).FullName);
            var result = new List<ViewsModel>();

            if (viewEngineFindViewsMessages == null || viewRenderMessages == null)
            {
                return result;
            }

            foreach (var findView in viewEngineFindViewsMessages)
            {
                result.Add(new ViewsModel(findView, viewRenderMessages.SingleOrDefault(r => r.ViewCorrelation.ViewEngineFindCallId == findView.Id)));
            }

            return result;
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
            {
                tabStore.Set(key, new List<T>());
            }

            var messages = tabStore.Get<IList<T>>(key);

            messages.Add(message);
        }
    }
}