using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.Framework
{
    public class GlimpseRuntime : IGlimpseRuntime
    {
        public GlimpseRuntime(GlimpseConfiguration configuration)
        {
            //Version is in major.minor.build format to support http://semver.org/
            //TODO: Consider adding configuration hash to version
            Version = GetType().Assembly.GetName().Version.ToString(3);
            UpdateConfiguration(configuration);
        }


        private GlimpseConfiguration Configuration { get; set; }
        
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

        public bool IsInitialized { get; private set; }

        public string Version { get; private set; }


        //TODO: Make sure runtime has been init'ed
        public void BeginRequest()
        {
            var policy = GetRuntimePolicy(RuntimeEvent.BeginRequest);
            if (policy == RuntimePolicy.Off) return;

            var requestStore = Configuration.FrameworkProvider.HttpRequestStore;

            //Give Request an ID
            requestStore.Set(Constants.RequestIdKey, Guid.NewGuid());

            //Create and start global stopwatch
            requestStore.Set(Constants.GlobalStopwatchKey, Stopwatch.StartNew());
        }

        //TODO: Add PRG support
        //TODO: Process MetaData
        //TODO: Sanitize JSON
        //TODO: Structured layout support
        public void EndRequest()
        {
            var policy = GetRuntimePolicy(RuntimeEvent.EndRequest);
            if (policy == RuntimePolicy.Off) return;

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
                throw new MethodAccessException(Resources.OutOfOrderRuntimeMethodCall, ex);
            }

            //TODO: Handle exceptions
            if (policy.HasFlag(RuntimePolicy.PersistResults))
            {
                var serializer = Configuration.Serializer;
                var pluginResults = TabResultsStore.ToDictionary(item => item.Key, item => serializer.Serialize(item.Value));
                var requestMetadata = frameworkProvider.RequestMetadata;

                var metadata = new GlimpseMetadata(requestId, requestMetadata, pluginResults, stopwatch.ElapsedMilliseconds);

                Configuration.PersistanceStore.Save(metadata);
            }

            if (policy.HasFlag(RuntimePolicy.ModifyResponseHeaders))
                frameworkProvider.SetHttpResponseHeader(Constants.HttpResponseHeader, requestId.ToString());

            if (policy.HasFlag(RuntimePolicy.DisplayGlimpseClient))
            {
                var html = GenerateScriptTags(requestId);

                frameworkProvider.InjectHttpResponseBody(html);
            }
        }

        internal string GenerateScriptTags(Guid requestId)
        {
            var encoder = Configuration.HtmlEncoder;
            var resourceEndpoint = Configuration.ResourceEndpoint;
            var clientScripts = Configuration.ClientScripts;
            var logger = Configuration.Logger;
            var resources = Configuration.Resources;

            var requestTokenValues = new Dictionary<string, string>
                                         {
                                             {ResourceParameterKey.RequestId, requestId.ToString()},
                                             {ResourceParameterKey.VersionNumber, Version}
                                         };

            var stringBuilder = new StringBuilder();

            foreach (var clientScript in clientScripts.OrderBy(cs=>cs.Order))
            {
                var dynamicScript = clientScript as IDynamicClientScript;
                if (dynamicScript != null)
                {
                    //TODO:Handle Exceptions
                    var resourceName = dynamicScript.GetResourceName();
                    var resource = resources.FirstOrDefault(r => r.Name.Equals(resourceName, StringComparison.InvariantCultureIgnoreCase));

                    if (resource == null)
                    {
                        logger.Warn(string.Format(Resources.RenderClientScriptMissingResourceWarning, clientScript.GetType(), resourceName));
                        continue;
                    }

                    var uri = encoder.HtmlAttributeEncode(resourceEndpoint.GenerateUri(resource, logger, requestTokenValues));
                    if(!string.IsNullOrWhiteSpace(uri))
                        stringBuilder.AppendFormat(@"<script type='text/javascript' src='{0}'></script>", uri);

                    continue;
                }

                var staticScript = clientScript as IStaticClientScript;
                if (staticScript != null)
                {
                    var uri = encoder.HtmlAttributeEncode(staticScript.GetUri(Version));

                    if(!string.IsNullOrWhiteSpace(uri))
                        stringBuilder.AppendFormat(@"<script type='text/javascript' src='{0}'></script>", uri);

                    continue;
                }

                logger.Warn(string.Format(Resources.RenderClientScriptImproperImplementationWarning, clientScript.GetType()));
            }

            return stringBuilder.ToString();
        }

        public void ExecuteDefaultResource()
        {
            ExecuteResource(Configuration.DefaultResourceName, new ResourceParameters());
        }

        public void ExecuteResource(string resourceName, IDictionary<string, string> namedParameters)
        {
            ExecuteResource(resourceName, new ResourceParameters{NamedParameters = namedParameters});
        }

        public void ExecuteResource(string resourceName, string[] orderedParameters)
        {
            ExecuteResource(resourceName, new ResourceParameters{OrderedParameters = orderedParameters});
        }

        public void ExecuteResource(string resourceName, ResourceParameters parameters)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(resourceName), "resourceName");
            Contract.Requires<ArgumentNullException>(parameters != null, "parameters");

            var policy = GetRuntimePolicy(RuntimeEvent.ExecuteResource);
            if (policy == RuntimePolicy.Off) return;

            var logger = Configuration.Logger;
            IResourceResult result;

            var resources =
                Configuration.Resources.Where(
                    r => r.Name.Equals(resourceName, StringComparison.InvariantCultureIgnoreCase));

            switch (resources.Count())
            {
                case 1: //200 - OK
                    try
                    {
                        var resource = resources.First();
                        var resourceContext = new ResourceContext(parameters.GetParametersFor(resource), Configuration.PersistanceStore, logger);
                        result = resources.First().Execute(resourceContext);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(string.Format("Error executing resource '{0}'", resourceName), ex);
                        result = new ExceptionResourceResult(ex);
                    }
                    break;
                case 0: //404 - File Not Found
                    logger.Warn(string.Format(Resources.ExecuteResourceMissingError, resourceName));
                    result = new StatusCodeResourceResult(404);
                    break;
                default: //500 - Server Error
                    logger.Warn(string.Format(Resources.ExecuteResourceDuplicateError, resourceName));
                    result = new StatusCodeResourceResult(500);
                    break;
            }

            try
            {
                var context = new ResourceResultContext(logger, Configuration.FrameworkProvider, Configuration.Serializer, Configuration.HtmlEncoder);
                result.Execute(context);
            }
            catch (Exception ex)
            {
                logger.Fatal(string.Format("Error executing resource result of type '{0}'", result.GetType()), ex);
            }
        }

        public void ExecuteTabs()
        {
            ExecuteTabs(LifeCycleSupport.EndRequest);
        }

        public void ExecuteTabs(LifeCycleSupport support)
        {
            var policy = GetRuntimePolicy(RuntimeEvent.ExecuteTabs);
            if (policy == RuntimePolicy.Off) return;

            var runtimeContext = Configuration.FrameworkProvider.RuntimeContext;
            var frameworkProviderRuntimeContextType = runtimeContext.GetType();

            //Only use tabs that either don't specify a specific context type, or have a context type that matches the current framework provider's.
            var runtimeTabs =
                Configuration.Tabs.Where(
                    p =>
                    p.Metadata.RequestContextType == null ||
                    frameworkProviderRuntimeContextType.IsSubclassOf(p.Metadata.RequestContextType) ||
                    p.Metadata.RequestContextType == frameworkProviderRuntimeContextType);

            var supportedRuntimeTabs = runtimeTabs.Where(p => p.Metadata.LifeCycleSupport.HasFlag(support));
            var tabResultsStore = TabResultsStore;
            var logger = Configuration.Logger;


            //Create storage space for tabs to access
            var tabStore = new DictionaryDataStoreAdapter(new Dictionary<string, object>());

            //Create ServiceLocator valid for this request
            var tabContext = new TabContext(runtimeContext, tabStore, Configuration.PipelineInspectors, logger);


            foreach (var tab in supportedRuntimeTabs)
            {
                var tabValue = tab.Value;
                var key = tabValue.GetType().FullName;
                try
                {
                    var result = new TabResult(tabValue.Name, tabValue.GetData(tabContext));

                    if (tabResultsStore.ContainsKey(key))
                        tabResultsStore[key] = result;
                    else
                        tabResultsStore.Add(key, result);
                }
                catch (Exception exception)
                {
                    logger.Error(string.Format(Resources.ExecuteTabError, key), exception);
                }
            }
        }

        public bool Initialize()
        {
            var logger = Configuration.Logger;
            var policy = GetRuntimePolicy(RuntimeEvent.Initialize);
            if (policy == RuntimePolicy.Off) return false;


            var tabsThatRequireSetup = Configuration.Tabs.Where(p => p.Value is ISetup).Select(p => p.Value);
            foreach (ISetup tab in tabsThatRequireSetup)
            {
                try
                {
                    tab.Setup();
                }
                catch (Exception exception)
                {
                    logger.Error(string.Format(Resources.InitializeTabError, tab.GetType()), exception);
                }
            }

            var pipelineInspectorContext = new PipelineInspectorContext(logger);

            foreach (var pipelineInspector in Configuration.PipelineInspectors)
            {
                try
                {
                    pipelineInspector.Setup(pipelineInspectorContext);
                }
                catch (Exception exception)
                {
                    logger.Error(
                        string.Format(Resources.InitializePipelineInspectorError, pipelineInspector.GetType()),
                        exception);
                }
            }

            IsInitialized = true;

            return policy != RuntimePolicy.Off;
        }

        //TODO: Test that these collections are auto populated
        public void UpdateConfiguration(GlimpseConfiguration configuration)
        {
            //TODO: destruct pipelineInitializers and tabs
            var tabs = configuration.Tabs;
            if (tabs.Discoverability.AutoDiscover)
                tabs.Discoverability.Discover();

            var pipelineInspectors = configuration.PipelineInspectors;
            if (pipelineInspectors.Discoverability.AutoDiscover)
                pipelineInspectors.Discoverability.Discover();

            var resources = configuration.Resources;
            if (resources.Discoverability.AutoDiscover)
                resources.Discoverability.Discover();

            var runtimePolicies = configuration.RuntimePolicies;
            if (runtimePolicies.Discoverability.AutoDiscover)
                runtimePolicies.Discoverability.Discover();

            var clientScripts = configuration.ClientScripts;
            if (clientScripts.Discoverability.AutoDiscover)
                clientScripts.Discoverability.Discover();

            var serializationConverters = configuration.SerializationConverters;
            if (serializationConverters.Discoverability.AutoDiscover)
            {
                serializationConverters.Discoverability.Discover();
                configuration.Serializer.RegisterSerializationConverters(serializationConverters);
            }

            Configuration = configuration;
        }

        private RuntimePolicy GetRuntimePolicy(RuntimeEvent runtimeEvent)
        {
            var frameworkProvider = Configuration.FrameworkProvider;
            var requestStore = frameworkProvider.HttpRequestStore;
            //Begin with the lowest policy for this request, or the lowest policy per config
            var result = requestStore.Contains(Constants.RuntimePermissionsKey)
                             ? requestStore.Get<RuntimePolicy>(Constants.RuntimePermissionsKey)
                             : Configuration.BasePolicy;

            if (result != RuntimePolicy.Off)
            {
                //only run policies for this runtimeEvent, or all runtime events
                var policies =
                    Configuration.RuntimePolicies.Where(
                        v => !v.Metadata.RuntimeEvent.HasValue || v.Metadata.RuntimeEvent.Value.HasFlag(runtimeEvent));

                var policyContext = new RuntimePolicyContext(frameworkProvider.RequestMetadata, Configuration.Logger, frameworkProvider.RuntimeContext);
                foreach (var policy in policies)
                {
                    //TODO: Handle exceptions from policy
                    var p = policy.Value.Execute(policyContext);

                    //Only use the lowest policy allowed for the request
                    if (p < result) result = p;
                }
            }

            //store result for request
            requestStore.Set(Constants.RuntimePermissionsKey, result);
            return result;
        }
    }
}