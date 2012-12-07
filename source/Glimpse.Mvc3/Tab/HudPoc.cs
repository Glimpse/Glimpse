using System;
using System.Collections.Generic;
using Glimpse.AspNet.Extensibility;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Mvc.AlternateImplementation;

namespace Glimpse.Mvc.Tab
{
    // Proof of concept for heads up display (hud) bar
    public class HudPoc : AspNetTab, ITabSetup, IKey
    {
        public override string Name
        {
            get { return "HUD"; }
        }

        public string Key 
        {
            get { return "glimpse_hud"; }
        }

        public override object GetData(ITabContext context)
        {
            return context.TabStore.Get<HudModel>();
        }

        public void Setup(ITabSetupContext context)
        {
            var mb = context.MessageBroker;

            mb.Subscribe<ActionInvoker.InvokeActionMethod.Message>(m => UpdateAction(m, context));
            mb.Subscribe<ViewEngine.FindViews.Message>(m => UpdateView(m, context));
            mb.Subscribe<View.Render.Message>(m => UpdateRender(m, context));
        }

        private void UpdateRender(View.Render.Message message, ITabSetupContext context)
        {
            var model = GetModel(context.GetTabStore());

            // Last message in is the first/primary view
            model.ViewRenderTime = message.Duration;
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
                model.ActionExecutionTime = message.Duration;
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

        private HudModel GetModel(IDataStore tabStore)
        {
            if (tabStore.Contains<HudModel>())
            {
                return tabStore.Get<HudModel>();
            }

            var model = new HudModel();
            tabStore.Set(model);
            return model;
        }
    }

    public class HudModel
    {
        public HudModel()
        {
            ChildViewCount = -1;
            MachineName = Environment.MachineName;
        }

        public string ActionName { get; set; }

        public double? ActionExecutionTime { get; set; }

        public int ChildActionCount { get; set; }

        public string ViewName { get; set; }

        public double? ViewRenderTime { get; set; }

        public int ChildViewCount { get; set; }

        public string MachineName { get; set; }

        public string ControllerName { get; set; }
    }

    public class HudModelConverter : SerializationConverter<HudModel>
    {
        public override object Convert(HudModel obj)
        {
            return new
                {
                    mvc = new Dictionary<string, object>
                        {
                            { "controllerName", obj.ControllerName },
                            { "actionName", obj.ActionName },
                            { "actionExecutionTime", Math.Round(obj.ActionExecutionTime.Value, 2) },
                            { "childActionCount", obj.ChildActionCount },
                            { "childViewCount", obj.ChildViewCount },
                            { "viewName", obj.ViewName },
                            { "viewRenderTime", Math.Round(obj.ViewRenderTime.Value, 2) },
                        },
                    environment = new Dictionary<string,object>
                        {
                            { "serverName", obj.MachineName },
                        }
                };
        }
    }
}