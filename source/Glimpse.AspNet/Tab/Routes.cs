using System.Collections.Generic;
using System.Linq;
using Glimpse.AspNet.AlternateType;
using Glimpse.AspNet.Extensibility;
using Glimpse.AspNet.Model;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Tab.Assist;
using MvcRoute = System.Web.Routing.Route;
using MvcRouteBase = System.Web.Routing.RouteBase;
using MvcRouteValueDictionary = System.Web.Routing.RouteValueDictionary;

namespace Glimpse.AspNet.Tab
{
    public class Routes : AspNetTab, IDocumentation, ITabSetup, ITabLayout, IKey
    {
        private static readonly List<string> AspNetProxiedRouteTypes = new List<string>
        {
            "Castle.Proxies.RouteProxy",
            "Castle.Proxies.RouteProxy_1",
            "Castle.Proxies.RouteBaseProxy",
            "Castle.Proxies.RouteBaseProxy_1"
        };
        
        private static readonly object Layout = TabLayout.Create()
                .Row(r =>
                {
                    r.Cell(0).WidthInPixels(100);
                    r.Cell(1).AsKey();
                    r.Cell(2);
                    r.Cell(3).WidthInPercent(20).SetLayout(TabLayout.Create().Row(x => 
                        {
                            x.Cell("{{0}} ({{1}})").WidthInPercent(45); 
                            x.Cell(2);
                        }));
                    r.Cell(4).WidthInPercent(35).SetLayout(TabLayout.Create().Row(x =>
                        {
                            x.Cell(0).WidthInPercent(30);
                            x.Cell(1);
                            x.Cell(2).WidthInPercent(30);
                        }));
                    r.Cell(5).WidthInPercent(15).SetLayout(TabLayout.Create().Row(x =>
                        {
                            x.Cell(0).WidthInPercent(45);
                            x.Cell(1).WidthInPercent(55); 
                        }));
                    r.Cell(6).WidthInPixels(100).Suffix(" ms").Class("mono").AlignRight();
                }).Build();

        public override string Name
        {
            get { return "Routes"; }
        }

        public string Key
        {
            get { return "glimpse_routes"; }
        }

        public string DocumentationUri
        {
            get { return "http://getGlimpse.com/Help/Routes-Tab"; }
        }

        public object GetLayout()
        {
            return Layout;
        }

        public void Setup(ITabSetupContext context)
        {
            context.PersistMessages<RouteBase.ProcessConstraint.Message>();
            context.PersistMessages<RouteBase.GetRouteData.Message>();
        }

        public override object GetData(ITabContext context)
        {
            var routeMessages = ProcessMessages(context.GetMessages<RouteBase.GetRouteData.Message>());
            var constraintMessages = ProcessMessages(context.GetMessages<RouteBase.ProcessConstraint.Message>());

            var result = new List<RouteModel>();
            
            using (System.Web.Routing.RouteTable.Routes.GetReadLock())
            {
                var aspNetRoutes = System.Web.Routing.RouteTable.Routes.Where(r => AspNetProxiedRouteTypes.Contains(r.GetType().ToString()));
                                        
                foreach (var routeBase in aspNetRoutes)
                {
                    var routeModel = GetRouteModelForRoute(context, routeBase, routeMessages, constraintMessages);
                    
                    result.Add(routeModel);
                }
            }

            return result;
        }

        private static TSource SafeFirstOrDefault<TSource>(IEnumerable<TSource> source)
        {
            if (source == null)
            {
                return default(TSource);
            }

            return source.FirstOrDefault();
        }

        private Dictionary<int, List<RouteBase.GetRouteData.Message>> ProcessMessages(IEnumerable<RouteBase.GetRouteData.Message> messages)
        { 
            if (messages == null)
            {
                return new Dictionary<int, List<RouteBase.GetRouteData.Message>>();
            }

            return messages.GroupBy(x => x.RouteHashCode).ToDictionary(x => x.Key, x => x.ToList());
        }

        private Dictionary<int, Dictionary<int, List<RouteBase.ProcessConstraint.Message>>> ProcessMessages(IEnumerable<RouteBase.ProcessConstraint.Message> messages)
        {
            if (messages == null)
            {
                return new Dictionary<int, Dictionary<int, List<RouteBase.ProcessConstraint.Message>>>();
            }

            return messages.GroupBy(x => x.RouteHashCode).ToDictionary(x => x.Key, x => x.ToList().GroupBy(y => y.ConstraintHashCode).ToDictionary(y => y.Key, y => y.ToList()));
        }

        private RouteModel GetRouteModelForRoute(ITabContext context, MvcRouteBase routeBase, Dictionary<int, List<RouteBase.GetRouteData.Message>> routeMessages, Dictionary<int, Dictionary<int, List<RouteBase.ProcessConstraint.Message>>> constraintMessages)
        {
            var routeModel = new RouteModel();

            var routeMessage = SafeFirstOrDefault(routeMessages.GetValueOrDefault(routeBase.GetHashCode()));
            if (routeMessage != null)
            {
                routeModel.Duration = routeMessage.Duration; 
                routeModel.IsMatch = routeMessage.IsMatch;
            }

            var route = routeBase as MvcRoute;
            if (route != null)
            {
                routeModel.Area = (route.DataTokens != null && route.DataTokens.ContainsKey("area")) ? route.DataTokens["area"].ToString() : null;
                routeModel.Url = route.Url;
                routeModel.RouteData = ProcessRouteData(route.Defaults, routeMessage);
                routeModel.Constraints = ProcessConstraints(context, route, constraintMessages);
                routeModel.DataTokens = ProcessDataTokens(route.DataTokens);
            }
            else
            {
                routeModel.Url = routeModel.ToString();
            }

            var routeName = routeBase as IRouteNameMixin;
            if (routeName != null)
            {
                routeModel.Name = routeName.Name;
            }

            return routeModel;
        }

        private IEnumerable<RouteDataItemModel> ProcessRouteData(MvcRouteValueDictionary dataDefaults, RouteBase.GetRouteData.Message routeMessage)
        {
            if (dataDefaults == null || dataDefaults.Count == 0)
            {
                return null;
            }

            var routeData = new List<RouteDataItemModel>();
            foreach (var dataDefault in dataDefaults)
            {
                var routeDataItemModel = new RouteDataItemModel();
                routeDataItemModel.PlaceHolder = dataDefault.Key;
                routeDataItemModel.DefaultValue = dataDefault.Value;

                if (routeMessage != null && routeMessage.Values != null)
                {
                    routeDataItemModel.ActualValue = routeMessage.Values[dataDefault.Key];
                }

                routeData.Add(routeDataItemModel);
            }

            return routeData;
        }

        private IEnumerable<RouteConstraintModel> ProcessConstraints(ITabContext context, MvcRoute route, Dictionary<int, Dictionary<int, List<RouteBase.ProcessConstraint.Message>>> constraintMessages)
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
                    var counstraintMessage = SafeFirstOrDefault(counstraintRouteMessages.GetValueOrDefault(constraint.Value.GetHashCode()));
                    model.IsMatch = false;
                    
                    if (counstraintMessage != null)
                    {
                        model.IsMatch = counstraintMessage.IsMatch;
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
    }
}
