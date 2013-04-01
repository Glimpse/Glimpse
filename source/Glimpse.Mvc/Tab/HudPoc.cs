using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq; 

using Glimpse.Ado.Message;
using Glimpse.Ado.Model; 
using Glimpse.AspNet.AlternateType;
using Glimpse.AspNet.Extensibility; 
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Mvc.AlternateType;

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
            var result = context.TabStore.Get<HudModel>();

            var messages = context.GetMessages<AdoMessage>().ToList();
            var aggregator = new MessageAggregator(messages);
            var queryData = aggregator.Aggregate();

            result.QueryCount = queryData.Commands.Count;
            result.ConnectionCount = queryData.Connections.Count;
            result.TransactionCount = queryData.Transactions.Count;

            foreach (var command in queryData.Commands)
            {
                var commandMetadata = command.Value;
                result.QueryExecutionTime += commandMetadata.Duration;
            }

            foreach (var connection in queryData.Connections)
            { 
                var connectionMetadata = connection.Value;
                result.ConnectionOpenTime += connectionMetadata.Duration.GetValueOrDefault(TimeSpan.Zero);
            }

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

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Just a POC - this is okay")]
    public class HudModel
    {
        public HudModel()
        {
            ChildViewCount = -1;
            MachineName = Environment.MachineName;
            QueryExecutionTime = TimeSpan.Zero;
        }

        public string ActionName { get; set; }

        public double? ActionExecutionTime { get; set; }

        public int ChildActionCount { get; set; }

        public string ViewName { get; set; }

        public double? ViewRenderTime { get; set; }

        public int ChildViewCount { get; set; }

        public string MachineName { get; set; }

        public string ControllerName { get; set; }

        public int QueryCount { get; set; }

        public TimeSpan QueryExecutionTime { get; set; }

        public int ConnectionCount { get; set; }

        public int TransactionCount { get; set; }

        public string MatchedRouteName { get; set; }

        public TimeSpan ConnectionOpenTime { get; set; }
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
                            { "matchedRouteName", obj.MatchedRouteName },
                        },
                    environment = new Dictionary<string, object>
                        {
                            { "serverName", obj.MachineName },
                        },
                    sql = new Dictionary<string, object>
                        {
                            { "queryCount", obj.QueryCount },
                            { "connectionCount", obj.ConnectionCount },
                            { "transactionCount", obj.TransactionCount },
                            { "queryExecutionTime", obj.QueryExecutionTime },
                            { "connectionOpenTime", obj.ConnectionOpenTime },
                        }
                };
        }
    }
}