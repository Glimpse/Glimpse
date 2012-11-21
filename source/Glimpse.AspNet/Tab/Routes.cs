using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Glimpse.AspNet.Extensibility;
using Glimpse.AspNet.Model;
using Glimpse.Core.Extensibility;
using Route = System.Web.Routing.Route;
using ProcessConstraint = Glimpse.AspNet.AlternateImplementation.Route.ProcessConstraint;

namespace Glimpse.AspNet.Tab
{
    public class Routes : AspNetTab, IDocumentation, ITabSetup
    {
        public string DocumentationUri
        {
            get { return "http://getGlimpse.com/Help/Plugin/Routes"; }
        }

        public override string Name
        {
            get { return "Routes"; }
        }

        /*
         * Implementation note: we have an entry in the tabstore for each
         * Route. Each entry is in turn a dictionary mapping from Constraint
         * (or rather, the key in the route's constraint dictionary, ie the
         * parameter name) to a Glimpse.AspNet.AlternateImplementation.Route.ProcessConstraint.Message.
         */

        public override object GetData(ITabContext context)
        {
            var result = new List<RouteModel>();

            var hasEverMatched = false;
            using (RouteTable.Routes.GetReadLock())
            {
                var httpContext = context.GetRequestContext<HttpContextBase>();
                foreach (var routeBase in RouteTable.Routes)
                {
                    var routeModel = routeBase is Route ? 
                        GetRouteModelForRoute(context, (Route)routeBase) : 
                        new RouteModel();
                    
                    RouteData routeData = null;
                    if(httpContext != null)
                        routeData = routeBase.GetRouteData(httpContext);

                    if (routeData != null)
                        AddActualValuesToRouteModel(routeModel, routeData.Values);

                    var matchesCurrentRequest = (routeData != null);
                    routeModel.IsFirstMatch = matchesCurrentRequest && !hasEverMatched;
                    routeModel.MatchesCurrentRequest = matchesCurrentRequest;

                    result.Add(routeModel);
                    hasEverMatched = hasEverMatched || matchesCurrentRequest;
                }
            }

            return result;
        }

        public RouteModel GetRouteModelForRoute(ITabContext context, Route route)
        {
            var data = new List<RouteDataItemModel>();

            if (route.Defaults != null)
            {
                data.AddRange(
                    from defaultItem in route.Defaults
                    select new RouteDataItemModel(defaultItem.Key, defaultItem.Value));
            }

            var area = "_Root_";

            if (route.DataTokens != null && route.DataTokens.ContainsKey("area"))
                area = route.DataTokens["area"].ToString();

            return new RouteModel(area,
                                route.Url, data.Count > 1 ? data : null,
                                GetConstraintsForRoute(context, route),
                                (route.DataTokens == null || route.DataTokens.Count == 0)
                                    ? null
                                    : route.DataTokens
                            );
        }

        private static IEnumerable<RouteConstraintModel> GetConstraintsForRoute(ITabContext context, Route route)
        {
            if (route.Constraints == null || route.Constraints.Count == 0)
                return null;

            var tabStoreKey = route.GetHashCode().ToString(CultureInfo.InvariantCulture);
            Dictionary<string,ProcessConstraint.Message> messages = null;
            if(context.TabStore.Contains(tabStoreKey))
                messages = context.TabStore.Get<Dictionary<string, ProcessConstraint.Message>>(tabStoreKey);

            var result = new List<RouteConstraintModel>();
            foreach (var constraint in route.Constraints)
            {
                string parameterName = constraint.Key;
                string test = constraint.Value.ToString();
            
                // see if there's an entry for this constraint in our tabstore (which would imply
                // it was checked).
                ProcessConstraint.Message msg = null;
                if (messages != null)
                    messages.TryGetValue(parameterName,out msg);
                
                var model = new RouteConstraintModel(parameterName, test,
                    msg != null, msg != null && msg.IsMatch);
                
                result.Add(model);
            }
            return result;
        }

        private static void AddActualValuesToRouteModel(RouteModel model, RouteValueDictionary values)
        {
            if(values == null || model.RouteData == null)
            {
                // either no data from this call, or no parameters in the dictionary.
                // nothing to do.
                return;
            }
                
            foreach(RouteDataItemModel item in model.RouteData)
            {
                item.ActualValue = values[item.PlaceHolder];
            }
        }

        public void Setup(ITabSetupContext context)
        {
            context.MessageBroker.Subscribe<ProcessConstraint.Message>(msg => Persist(msg, context));
        }

        public static void Persist(ProcessConstraint.Message msg,
            ITabSetupContext context)
        {
            var ts = context.GetTabStore();
            var key = msg.Route.GetHashCode().ToString(CultureInfo.InvariantCulture);

            Dictionary<string, ProcessConstraint.Message> tabStoreDictionary;
            if(!ts.Contains(key))
            {
                tabStoreDictionary = new Dictionary<string, ProcessConstraint.Message>();
                ts.Set(key, tabStoreDictionary);
            }
            else
            {
                tabStoreDictionary = ts.Get<Dictionary<string, ProcessConstraint.Message>>(key);
            }

            tabStoreDictionary[msg.ParameterName] = msg;
        }
    }
}