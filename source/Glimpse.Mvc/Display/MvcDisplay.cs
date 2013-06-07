using System;
using Glimpse.AspNet.AlternateType;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Mvc.AlternateType;
using Glimpse.Mvc.Model;

namespace Glimpse.Mvc.Display
{
    public class MvcDisplay : IDisplay, ITabSetup, IKey
    {
        private const string InternalName = "mvc";

        public string Name
        {
            get { return InternalName; }
        }

        public string Key
        {
            get { return InternalName; }
        }

        public object GetData(ITabContext context)
        {
            var result = context.TabStore.Get<MvcDisplayModel>();

            return result;
        }

        public void Setup(ITabSetupContext context)
        {
            var mb = context.MessageBroker;

            mb.Subscribe<ActionInvoker.InvokeActionMethod.Message>(m => UpdateAction(m, context));
            mb.Subscribe<ViewEngine.FindViews.Message>(m => UpdateView(m, context));
            mb.Subscribe<View.Render.Message>(m => UpdateRender(m, context));
            mb.Subscribe<RouteBase.GetRouteData.Message>(m => UpdateRoute(m, context));
        }

        private void UpdateRoute(RouteBase.GetRouteData.Message message, ITabSetupContext context)
        {
            var model = GetModel(context.GetTabStore());

            // string.Empty is a valid routeName
            if (message.IsMatch && model.MatchedRouteName == null)
            {
                // only update the first matched route
                model.MatchedRouteName = message.RouteName;
            }
        }

        private void UpdateRender(View.Render.Message message, ITabSetupContext context)
        {
            var model = GetModel(context.GetTabStore());

            // Last message in is the first/primary view
            model.ViewRenderTime = Math.Round(message.Duration.TotalMilliseconds, 2);
        }

        private void UpdateAction(ActionInvoker.InvokeActionMethod.Message message, ITabSetupContext context)
        {
            var model = GetModel(context.GetTabStore());

            if (message.IsChildAction)
            {
                model.ChildActionCount++;
            }
            else
            {
                model.ActionName = message.ActionName;
                model.ActionExecutionTime = Math.Round(message.Duration.TotalMilliseconds, 2);
                model.ControllerName = message.ControllerName;
            }
        }

        private void UpdateView(ViewEngine.FindViews.Message message, ITabSetupContext context)
        {
            if (message.IsFound)
            {
                var model = GetModel(context.GetTabStore());

                model.ChildViewCount++;

                if (model.ViewName == null)
                {
                    model.ViewName = message.ViewName;
                }
            }
        }

        private MvcDisplayModel GetModel(IDataStore tabStore)
        {
            if (tabStore.Contains<MvcDisplayModel>())
            {
                return tabStore.Get<MvcDisplayModel>();
            }

            var model = new MvcDisplayModel();
            tabStore.Set(model);
            return model;
        }
    }
}