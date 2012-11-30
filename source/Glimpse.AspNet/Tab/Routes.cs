using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Glimpse.AspNet.AlternateImplementation;
using Glimpse.AspNet.Extensibility;
using Glimpse.AspNet.Model;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Message;

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
        }

        public override object GetData(ITabContext context)
        {
            var routeMessages = context.TabStore.Get<IEnumerable<Route.ProcessConstraint.Message>>(typeof(Route.ProcessConstraint.Message).FullName);

            var result = new List<RouteModel>();

            var httpContext = context.GetRequestContext<HttpContextBase>();
            var hasEverMatched = false;

            using (System.Web.Routing.RouteTable.Routes.GetReadLock())
            {
                foreach (var routeBase in System.Web.Routing.RouteTable.Routes)
                {
                    var route = routeBase as System.Web.Routing.Route;

                    var routeModel = route != null ? GetRouteModelForRoute(context, route, routeMessages) : new RouteModel();

                    System.Web.Routing.RouteData routeData = null;
                    if (httpContext != null)
                    {
                        ////routeData = routeBase.GetRouteData(httpContext);
                        ////if (routeData != null)
                        ////{
                        ////    ProcessValues(routeModel, routeData.Values);
                        ////}
                    }

                    var matchesCurrentRequest = routeData != null;
                    routeModel.IsFirstMatch = matchesCurrentRequest && !hasEverMatched;
                    routeModel.MatchesCurrentRequest = matchesCurrentRequest;

                    result.Add(routeModel);

                    hasEverMatched = hasEverMatched || matchesCurrentRequest;
                }
            }

            return result;
        }

        public RouteModel GetRouteModelForRoute(ITabContext context, System.Web.Routing.Route route, IEnumerable<Route.ProcessConstraint.Message> routeMessages)
        {
            var routeData = new List<RouteDataItemModel>();
            if (route.Defaults != null)
            {
                routeData.AddRange(route.Defaults.Select(x => new RouteDataItemModel(x.Key, x.Value)));
            }

            var result = new RouteModel();
            result.Area = (route.DataTokens != null && route.DataTokens.ContainsKey("area")) ? route.DataTokens["area"].ToString() : null;
            result.Url = route.Url;
            result.RouteData = routeData.Count > 1 ? routeData : null;
            result.Constraints = ProcessConstraints(context, route, routeMessages);
            result.DataTokens = (route.DataTokens != null && route.DataTokens.Count > 0) ? route.DataTokens : null;

            return result;
        }

        private IEnumerable<RouteConstraintModel> ProcessConstraints(ITabContext context, System.Web.Routing.Route route, IEnumerable<Route.ProcessConstraint.Message> routeMessages)
        {
            if (route.Constraints == null || route.Constraints.Count == 0)
            {
                return null;
            }

            var result = new List<RouteConstraintModel>();
            foreach (var constraint in route.Constraints)
            {
                var model = new RouteConstraintModel();
                model.ParameterName = constraint.Key;
                model.Constraint = constraint.Value.ToString(); 

                result.Add(model);
            }

            return result;
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