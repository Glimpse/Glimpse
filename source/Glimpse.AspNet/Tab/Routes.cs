using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Glimpse.AspNet.AlternateImplementation;
using Glimpse.AspNet.Extensibility;
using Glimpse.AspNet.Model;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Message;
using MvcRoute = System.Web.Routing.Route;
using MvcRouteBase = System.Web.Routing.RouteBase;
using MvcRouteValueDictionary = System.Web.Routing.RouteValueDictionary;

namespace Glimpse.AspNet.Tab
{

    public class Routes : AspNetTab, IDocumentation, ITabSetup
    {
        public override string Name
        {
            get { return "Routes"; }
        }

        public string DocumentationUri
        {
            get { return "http://getGlimpse.com/Help/Plugin/Routes"; }
        }

        public void Setup(ITabSetupContext context)
        {
            context.MessageBroker.Subscribe<Route.ProcessConstraint.Message>(msg => Persist(msg, context));
            context.MessageBroker.Subscribe<RouteBase.GetRouteData.Message>(msg => Persist(msg, context));
        }

        public override object GetData(ITabContext context)
        {
            var routeMessages = ProcessMessages(context.TabStore.Get<List<RouteBase.GetRouteData.Message>>(typeof(RouteBase.GetRouteData.Message).FullName));
            var constraintMessages = ProcessMessages(context.TabStore.Get<List<Route.ProcessConstraint.Message>>(typeof(Route.ProcessConstraint.Message).FullName));

            var result = new List<RouteModel>();

            using (System.Web.Routing.RouteTable.Routes.GetReadLock())
            {
                foreach (var routeBase in System.Web.Routing.RouteTable.Routes)
                {
                    var routeModel = GetRouteModelForRoute(context, routeBase, routeMessages, constraintMessages);

                    result.Add(routeModel);
                }
            }

            return result;
        }

        private Dictionary<int, List<RouteBase.GetRouteData.Message>> ProcessMessages(IEnumerable<RouteBase.GetRouteData.Message> messages)
        { 
            return messages.GroupBy(x => x.RouteHashCode).ToDictionary(x => x.Key, x => x.ToList());
        }

        private Dictionary<int, Dictionary<int, List<Route.ProcessConstraint.Message>>> ProcessMessages(IEnumerable<Route.ProcessConstraint.Message> messages)
        { 
            return messages.GroupBy(x => x.RouteHashCode).ToDictionary(x => x.Key, x => x.ToList().GroupBy(y => y.ConstraintHashCode).ToDictionary(y => y.Key, y => y.ToList()));
        }

        private RouteModel GetRouteModelForRoute(ITabContext context, MvcRouteBase routeBase, Dictionary<int, List<RouteBase.GetRouteData.Message>> routeMessages, Dictionary<int, Dictionary<int, List<Route.ProcessConstraint.Message>>> constraintMessages)
        {
            var result = new RouteModel();

            var route = routeBase as MvcRoute;
            if (route != null)
            {
                result.Area = (route.DataTokens != null && route.DataTokens.ContainsKey("area")) ? route.DataTokens["area"].ToString() : null;
                result.Url = route.Url;
                result.RouteData = ProcessRouteData(route.Defaults);
                result.Constraints = ProcessConstraints(context, route, constraintMessages);
                result.DataTokens = ProcessDataTokens(route.DataTokens);
            }
            else
            {
                result.Url = "Can't show non System.Web.MVC.Route types";
            }

            var routeMessage = routeMessages.GetValueOrDefault(routeBase.GetHashCode()).SafeFirstOrDefault();
            if (routeMessage != null)
            {
                result.Duration = routeMessage.Duration;
                result.IsFirstMatch = routeMessage.IsMatch;
            }

            return result;
        }

        private IEnumerable<RouteDataItemModel> ProcessRouteData(MvcRouteValueDictionary defaults)
        {
            if (defaults == null || defaults.Count == 0)
            {
                return null;
            }

            var routeData = new List<RouteDataItemModel>();
            routeData.AddRange(defaults.Select(x => new RouteDataItemModel(x.Key, x.Value)));

            return routeData;
        }

        private IEnumerable<RouteConstraintModel> ProcessConstraints(ITabContext context, MvcRoute route, Dictionary<int, Dictionary<int, List<Route.ProcessConstraint.Message>>> constraintMessages)
        {
            if (route.Constraints == null || route.Constraints.Count == 0)
            {
                return null;
            }
             
            var counstraintRouteMessages = constraintMessages.GetValueOrDefault(route.GetHashCode()); 

            var result = new List<RouteConstraintModel>();
            foreach (var constraint in route.Constraints)
            {
                var model = new RouteConstraintModel();
                model.ParameterName = constraint.Key;
                model.Constraint = constraint.Value.ToString();

                if (counstraintRouteMessages != null)
                {
                    var counstraintMessage = counstraintRouteMessages.GetValueOrDefault(route.GetHashCode()).SafeFirstOrDefault();
                    model.Checked = true;
                    if (counstraintMessage != null)
                    {
                        model.Matched = counstraintMessage.IsMatch;
                    }
                }

                result.Add(model);
            }

            return result;
        }

        private IDictionary<string, object> ProcessDataTokens(IDictionary<string, object> dataTokens)
        {
            return dataTokens != null && dataTokens.Count > 0 ? dataTokens : null;
        }

        private void ProcessValues(RouteModel model, System.Web.Routing.RouteValueDictionary values)
        {
            if (values == null || model.RouteData == null)
            {
                return;
            }

            foreach (var item in model.RouteData)
            {
                item.ActualValue = values[item.PlaceHolder];
            }
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