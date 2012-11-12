using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Plugin.Assist;
using Glimpse.Core.ResourceResult;
#if NET35
using Glimpse.Core.Backport;
#endif

namespace Glimpse.Core.Framework
{
    public class GlimpseRuntime : IGlimpseRuntime
    {
        private static object lockObj = new object();

        static GlimpseRuntime()
        {
            // Version is in major.minor.build format to support http://semver.org/
            // TODO: Consider adding configuration hash to version
            Version = Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
        }

        public GlimpseRuntime(IGlimpseConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static string Version { get; private set; }

        public IGlimpseConfiguration Configuration { get; set; }

        public bool IsInitialized { get; private set; }

        private IDictionary<string, TabResult> TabResultsStore
        {
            get
            {
                var requestStore = Configuration.FrameworkProvider.HttpRequestStore;
                var result = requestStore.Get<IDictionary<string, TabResult>>(Constants.PluginResultsDataStoreKey);

                if (result == null)
                {
                    result = new Dictionary<string, TabResult>();
                    requestStore.Set(Constants.PluginResultsDataStoreKey, result);
                }

                return result;
            }
        }

        public void BeginRequest()
        {
            if (!IsInitialized)
            {
                throw new GlimpseException(Resources.BeginRequestOutOfOrderRuntimeMethodCall);
            }

            var policy = GetRuntimePolicy(RuntimeEvent.BeginRequest);
            if (policy.HasFlag(RuntimePolicy.Off))
            {
                return;
            }

            ExecuteTabs(RuntimeEvent.BeginRequest);

            var requestStore = Configuration.FrameworkProvider.HttpRequestStore;

            // Give Request an ID
            requestStore.Set(Constants.RequestIdKey, Guid.NewGuid());

            // Create and start global stopwatch
            var stopwatch = Stopwatch.StartNew();
            requestStore.Set(Constants.GlobalStopwatchKey, stopwatch);
            requestStore.Set(Constants.GlobalTimerKey, new ExecutionTimer(stopwatch));
        }

        // TODO: Add PRG support
        // TODO: Structured layout support (TabLayout)
        public void EndRequest()
        {
            var policy = GetRuntimePolicy(RuntimeEvent.EndRequest);
            if (policy.HasFlag(RuntimePolicy.Off))
            {
                return;
            }

            ExecuteTabs(RuntimeEvent.EndRequest);

            var frameworkProvider = Configuration.FrameworkProvider;
            var requestStore = frameworkProvider.HttpRequestStore;
            
            Guid requestId;
            Stopwatch stopwatch;
            try
            {
                requestId = requestStore.Get<Guid>(Constants.RequestIdKey);
                stopwatch = requestStore.Get<Stopwatch>(Constants.GlobalStopwatchKey);
                stopwatch.Stop();
            }
            catch (NullReferenceException ex)
            {
                throw new GlimpseException(Resources.EndRequestOutOfOrderRuntimeMethodCall, ex);
            }

            var requestMetadata = frameworkProvider.RequestMetadata;
            if (policy.HasFlag(RuntimePolicy.PersistResults))
            {
                var persistenceStore = Configuration.PersistenceStore;

                var metadata = new GlimpseRequest(requestId, requestMetadata, TabResultsStore, stopwatch.ElapsedMilliseconds);

                try
                {
                    persistenceStore.Save(metadata);
                }
                catch (Exception exception)
                {
                    Configuration.Logger.Error(Resources.GlimpseRuntimeEndRequesPersistError, exception, persistenceStore.GetType());
                }
            }

            if (policy.HasFlag(RuntimePolicy.ModifyResponseHeaders))
            {
                frameworkProvider.SetHttpResponseHeader(Constants.HttpResponseHeader, requestId.ToString());

                if (requestMetadata.GetCookie(Constants.ClientIdCookieName) == null)
                {
                    frameworkProvider.SetCookie(Constants.ClientIdCookieName, requestMetadata.ClientId);
                }
            }

            if (policy.HasFlag(RuntimePolicy.DisplayGlimpseClient))
            {
                var html = Configuration.GenerateScriptTags(requestId, Version);

                frameworkProvider.InjectHttpResponseBody(html);
            }
        }

        public void ExecuteDefaultResource()
        {
            ExecuteResource(Configuration.DefaultResource.Name, ResourceParameters.None());
        }

        public void BeginSessionAccess()
        {
            var policy = GetRuntimePolicy(RuntimeEvent.BeginSessionAccess);
            if (policy.HasFlag(RuntimePolicy.Off))
            {
                return;
            }

            ExecuteTabs(RuntimeEvent.BeginSessionAccess);
        }

        public void EndSessionAccess()
        {
            var policy = GetRuntimePolicy(RuntimeEvent.EndSessionAccess);
            if (policy.HasFlag(RuntimePolicy.Off))
            {
                return;
            }

            ExecuteTabs(RuntimeEvent.EndSessionAccess);
        }

        public void ExecuteResource(string resourceName, ResourceParameters parameters)
        {
            if (string.IsNullOrEmpty(resourceName))
            {
                throw new ArgumentNullException("resourceName");
            }

            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            
            var logger = Configuration.Logger;
            var context = new ResourceResultContext(logger, Configuration.FrameworkProvider, Configuration.Serializer, Configuration.HtmlEncoder);
            IResourceResult result;

            var policy = GetRuntimePolicy(RuntimeEvent.ExecuteResource);
            if (policy == RuntimePolicy.Off && !resourceName.Equals(Configuration.DefaultResource.Name))
            {
                var message = string.Format(Resources.ExecuteResourceInsufficientPolicy, resourceName);
                logger.Info(message);
                new StatusCodeResourceResult(403, message).Execute(context);
                return;
            }

            var resources =
                Configuration.Resources.Where(
                    r => r.Name.Equals(resourceName, StringComparison.InvariantCultureIgnoreCase));

            switch (resources.Count())
            {
                case 1: // 200 - OK
                    try
                    {
                        var resource = resources.First();
                        var resourceContext = new ResourceContext(parameters.GetParametersFor(resource), Configuration.PersistenceStore, logger);

                        var privilegedResource = resource as IPrivilegedResource;

                        if (privilegedResource != null)
                        {
                            result = privilegedResource.Execute(resourceContext, Configuration);
                        }
                        else
                        {
                            result = resource.Execute(resourceContext);
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error(Resources.GlimpseRuntimeExecuteResourceError, ex, resourceName);
                        result = new ExceptionResourceResult(ex);
                    }

                    break;
                case 0: // 404 - File Not Found
                    logger.Warn(Resources.ExecuteResourceMissingError, resourceName);
                    result = new StatusCodeResourceResult(404);
                    break;
                default: // 500 - Server Error
                    logger.Warn(Resources.ExecuteResourceDuplicateError, resourceName);
                    result = new StatusCodeResourceResult(500);
                    break;
            }

            try
            {
                result.Execute(context);
            }
            catch (Exception exception)
            {
                logger.Fatal(Resources.GlimpseRuntimeExecuteResourceResultError, exception, result.GetType());
            }
        }

        public bool Initialize()
        {
            var logger = Configuration.Logger;
            var policy = GetRuntimePolicy(RuntimeEvent.Initialize);
            if (policy == RuntimePolicy.Off)
            {
                return false;
            }

            // Double checked lock to ensure thread safety. http://en.wikipedia.org/wiki/Double_checked_locking_pattern
            if (!IsInitialized)
            {
                lock (lockObj)
                {
                    if (!IsInitialized)
                    {
                        var messageBroker = Configuration.MessageBroker;
                        
                        var tabsThatRequireSetup = Configuration.Tabs.Where(tab => tab is ITabSetup).Select(tab => tab);
                        foreach (ITabSetup tab in tabsThatRequireSetup)
                        {
                            var key = tab.GetType().ConvertToSafeJson();
                            try
                            {
                                var setupContext = new TabSetupContext(logger, messageBroker, () => GetTabStore(key));
                                tab.Setup(setupContext);
                            }
                            catch (Exception exception)
                            {
                                logger.Error(Resources.InitializeTabError, exception, key);
                            }
                        }

                        var pipelineInspectorContext = new PipelineInspectorContext(logger, Configuration.ProxyFactory, messageBroker, Configuration.TimerStrategy, Configuration.RuntimePolicyStrategy);

                        foreach (var pipelineInspector in Configuration.PipelineInspectors)
                        {
                            try
                            {
                                pipelineInspector.Setup(pipelineInspectorContext);
                                logger.Debug(Resources.GlimpseRuntimeInitializeSetupPipelineInspector, pipelineInspector.GetType());
                            }
                            catch (Exception exception)
                            {
                                logger.Error(Resources.InitializePipelineInspectorError, exception, pipelineInspector.GetType());
                            }
                        }

                        PersistMetadata();

                        IsInitialized = true;
                    }
                }
            }

            return policy != RuntimePolicy.Off;
        }

        private IDataStore GetTabStore(string tabName)
        {
            var requestStore = Configuration.FrameworkProvider.HttpRequestStore;

            if (!requestStore.Contains(Constants.TabStorageKey))
            {
                requestStore.Set(Constants.TabStorageKey, new Dictionary<string, IDataStore>());
            }

            var tabStorage = requestStore.Get<IDictionary<string, IDataStore>>(Constants.TabStorageKey);

            if (!tabStorage.ContainsKey(tabName))
            {
                tabStorage.Add(tabName, new DictionaryDataStoreAdapter(new Dictionary<string, object>()));
            }

            return tabStorage[tabName];
        }

        private void ExecuteTabs(RuntimeEvent runtimeEvent)
        {
            var runtimeContext = Configuration.FrameworkProvider.RuntimeContext;
            var frameworkProviderRuntimeContextType = runtimeContext.GetType();
            var messageBroker = Configuration.MessageBroker;

            // Only use tabs that either don't specify a specific context type, or have a context type that matches the current framework provider's.
            var runtimeTabs =
                Configuration.Tabs.Where(
                    tab =>
                    tab.RequestContextType == null ||
                    frameworkProviderRuntimeContextType.IsSubclassOf(tab.RequestContextType) ||
                    tab.RequestContextType == frameworkProviderRuntimeContextType);

            var supportedRuntimeTabs = runtimeTabs.Where(p => p.ExecuteOn.HasFlag(runtimeEvent));
            var tabResultsStore = TabResultsStore;
            var logger = Configuration.Logger;

            foreach (var tab in supportedRuntimeTabs)
            {
                var key = tab.GetType().ConvertToSafeJson();
                try
                {
                    var tabContext = new TabContext(runtimeContext, GetTabStore(key), logger, messageBroker);
                    var tabData = tab.GetData(tabContext);

                    var tabSection = tabData as TabSection;
                    if (tabSection != null)
                    {
                        tabData = tabSection.Build();
                    }
                    
                    var result = new TabResult(tab.Name, tabData);

                    if (tabResultsStore.ContainsKey(key))
                    {
                        tabResultsStore[key] = result;
                    }
                    else
                    {
                        tabResultsStore.Add(key, result);
                    }
                }
                catch (Exception exception)
                {
                    logger.Error(Resources.ExecuteTabError, exception, key);
                }
            }
        }

        private void PersistMetadata()
        {
            var metadata = new GlimpseMetadata { Version = Version };
            var pluginMetadata = metadata.Plugins;

            foreach (var tab in Configuration.Tabs)
            {
                var metadataInstance = new PluginMetadata();
                var documentationTab = tab as IDocumentation;
                if (documentationTab != null)
                {
                    metadataInstance.DocumentationUri = documentationTab.DocumentationUri;
                } 

                if (metadataInstance.HasMetadata)
                {
                    pluginMetadata[tab.GetType().ConvertToSafeJson()] = metadataInstance;
                } 
            }

            var resources = metadata.Resources;
            var endpoint = Configuration.ResourceEndpoint;
            var logger = Configuration.Logger;

            foreach (var resource in Configuration.Resources)
            {
                if (resources.ContainsKey(resource.Name))
                {
                    logger.Warn(Resources.GlimpseRuntimePersistMetadataMultipleResourceWarning, resource.Name);
                }

                resources[resource.Name] = endpoint.GenerateUriTemplate(resource, Configuration.EndpointBaseUri, logger);
            }

            Configuration.PersistenceStore.Save(metadata);
        }

        private RuntimePolicy GetRuntimePolicy(RuntimeEvent runtimeEvent)
        {
            var frameworkProvider = Configuration.FrameworkProvider;
            var requestStore = frameworkProvider.HttpRequestStore;
            
            // Begin with the lowest policy for this request, or the lowest policy per config
            var finalResult = requestStore.Contains(Constants.RuntimePolicyKey)
                             ? requestStore.Get<RuntimePolicy>(Constants.RuntimePolicyKey)
                             : Configuration.DefaultRuntimePolicy;

            if (!finalResult.HasFlag(RuntimePolicy.Off))
            {
                var logger = Configuration.Logger;
                
                // only run policies for this runtimeEvent, or all runtime events
                var policies =
                    Configuration.RuntimePolicies.Where(
                        policy => policy.ExecuteOn.HasFlag(runtimeEvent));

                var policyContext = new RuntimePolicyContext(frameworkProvider.RequestMetadata, Configuration.Logger, frameworkProvider.RuntimeContext);
                foreach (var policy in policies)
                {
                    var policyResult = RuntimePolicy.Off;
                    try
                    {
                        policyResult = policy.Execute(policyContext);

                        if (policyResult != RuntimePolicy.On)
                        {
                            logger.Debug("RuntimePolicy set to '{0}' by IRuntimePolicy of type '{1}' during RuntimeEvent '{2}'.", policyResult, policy.GetType(), runtimeEvent);
                        }
                    }
                    catch (Exception exception)
                    {
                        logger.Warn("Exception when executing IRuntimePolicy of type '{0}'. RuntimePolicy is now set to 'Off'.", exception, policy.GetType());
                    }

                    // Only use the lowest policy allowed for the request
                    if (policyResult < finalResult)
                    {
                        finalResult = policyResult;
                    }
                }
            }

            // store result for request
            requestStore.Set(Constants.RuntimePolicyKey, finalResult);
            return finalResult;
        }
    }
}