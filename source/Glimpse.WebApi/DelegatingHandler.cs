//using System.Net.Http;
//using System.Threading;
//using System.Threading.Tasks;
//using System;
//using System.Threading;
//using System.Web;
//using System.Web.Compilation;
//using Glimpse.Core.Extensibility;
//using Glimpse.Core.Framework;
//using System.Web.Http.Controllers;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using Glimpse.Core.Extensibility;
//using Glimpse.Core.Extensions;
//using Glimpse.Core.Message;
//using Glimpse.Core.Resource;
//using Glimpse.Core.ResourceResult;
//using Glimpse.Core.Tab.Assist;

//namespace Glimpse.WebApi
//{
//    public class GlimpseHandler : DelegatingHandler
//    {
//        private static readonly object LockObj = new object();
//        private static readonly Factory Factory;

//        public static HttpRequestContext CurrentRequestContext;

//        static GlimpseHandler()
//        {
//            var serviceLocator = new WebApiServiceLocator();
//            Factory = new Factory(serviceLocator);
//            ILogger logger = Factory.InstantiateLogger();
//            serviceLocator.Logger = logger;
//        }

//        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
//        {
//            var currentRequestContext = request.GetRequestContext();
//            var runtime = GetRuntime(new HttpContextWrapper(HttpContext.Current).Application);
//            runtime.BeginRequest();

//            var response = await base.SendAsync(request, cancellationToken);

//            runtime.EndRequest();

//            return response;
//        }

//        internal void BeginRequest(this GlimpseRuntime runtime,HttpRequestContext context)
//        {
//            if (!runtime.IsInitialized)
//            {
//                throw new GlimpseException(Glimpse.Core.Resources.BeginRequestOutOfOrderRuntimeMethodCall);
//            }

//            //if (HasOffRuntimePolicy(RuntimeEvent.BeginRequest))
//            //    return;

//            ExecuteTabs(RuntimeEvent.BeginRequest,context, runtime);

//            var requestStore = runtime.Configuration.FrameworkProvider.HttpRequestStore;

//            // Give Request an ID
//            var requestId = Guid.NewGuid();
//            requestStore.Set(Glimpse.Core.Constants.RequestIdKey, requestId);
//            //Func<Guid?, string> generateClientScripts = (rId) => rId.HasValue ? GenerateScriptTags(rId.Value) : GenerateScriptTags(requestId);
//            //requestStore.Set(Glimpse.Core.Constants.ClientScriptsStrategy, generateClientScripts);

//            //var executionTimer = runtime.CreateAndStartGlobalExecutionTimer(requestStore);

//            //runtime.Configuration.MessageBroker.Publish(new RuntimeMessage().AsSourceMessage(typeof(GlimpseRuntime), MethodInfoBeginRequest).AsTimelineMessage("Start Request", TimelineCategory.Request).AsTimedMessage(executionTimer.Point()));

//        }

//        private void ExecuteTabs(RuntimeEvent runtimeEvent, HttpRequestContext runtimeContext, GlimpseRuntime runtime)
//        {
//            //var runtimeContext = Configuration.FrameworkProvider.RuntimeContext;
//            var frameworkProviderRuntimeContextType = runtimeContext.GetType();
//            var messageBroker = runtime.Configuration.MessageBroker;

//            // Only use tabs that either don't specify a specific context type, or have a context type that matches the current framework provider's.
//            var runtimeTabs =
//                runtime.Configuration.Tabs.Where(
//                    tab =>
//                    tab.RequestContextType == null ||
//                    frameworkProviderRuntimeContextType.IsSubclassOf(tab.RequestContextType) ||
//                    tab.RequestContextType == frameworkProviderRuntimeContextType);

//            var supportedRuntimeTabs = runtimeTabs.Where(p => p.ExecuteOn.HasFlag(runtimeEvent));
//            //var tabResultsStore = runtime.TabResultsStore;
//            var logger = runtime.Configuration.Logger;

//            foreach (var tab in supportedRuntimeTabs)
//            {
//                TabResult result;
//                var key = CreateKey(tab);
//                try
//                {
//                    var tabContext = new TabContext(runtimeContext, GetTabStore(key), logger, messageBroker);
//                    var tabData = tab.GetData(tabContext);

//                    var tabSection = tabData as TabSection;
//                    if (tabSection != null)
//                    {
//                        tabData = tabSection.Build();
//                    }

//                    result = new TabResult(tab.Name, tabData);
//                }
//                catch (Exception exception)
//                {
//                    result = new TabResult(tab.Name, exception.ToString());
//                    logger.Error(Resources.ExecuteTabError, exception, key);
//                }

//                if (tabResultsStore.ContainsKey(key))
//                {
//                    tabResultsStore[key] = result;
//                }
//                else
//                {
//                    tabResultsStore.Add(key, result);
//                }
//            }
//        }


//        internal IGlimpseRuntime GetRuntime(HttpApplicationStateBase applicationState)
//        {
//            var runtime = applicationState[Constants.RuntimeKey] as IGlimpseRuntime;

//            if (runtime == null)
//            {
//                lock (LockObj)
//                {
//                    runtime = applicationState[Constants.RuntimeKey] as IGlimpseRuntime;

//                    if (runtime == null)
//                    {
//                        runtime = Factory.InstantiateRuntime();

//                        applicationState.Add(Constants.RuntimeKey, runtime);
//                    }
//                }
//            }

//            return runtime;
//        }

//    }
//}
