using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Message;
using Glimpse.Core.ResourceResult;
using Glimpse.Core.Tab.Assist;
#if NET35
using Glimpse.Core.Backport;
#endif

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// The heart and soul of Glimpse. The runtime coordinate all input from a <see cref="IRequestResponseAdapter" />, persists collected runtime information and writes responses out to the <see cref="IRequestResponseAdapter" />.
    /// </summary>
    public class GlimpseRuntime : IGlimpseRuntime
    {
        private static readonly MethodInfo MethodInfoBeginRequest = typeof(GlimpseRuntime).GetMethod("BeginRequest", BindingFlags.Public | BindingFlags.Instance);
        private static readonly MethodInfo MethodInfoEndRequest = typeof(GlimpseRuntime).GetMethod("EndRequest", BindingFlags.Public | BindingFlags.Instance);
        private static readonly object LockObj = new object();
        private static GlimpseRuntime instance;

        /// <summary>
        /// Initializes static members of the <see cref="GlimpseRuntime" /> class.
        /// </summary>
        /// <exception cref="System.NullReferenceException">BeginRequest method not found</exception>
        static GlimpseRuntime()
        {
            // Version is in major.minor.build format to support http://semver.org/
            // TODO: Consider adding configuration hash to version
            Version = Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
            IsInitialized = false;

            if (MethodInfoBeginRequest == null)
            {
                throw new NullReferenceException("BeginRequest method not found");
            }

            if (MethodInfoEndRequest == null)
            {
                throw new NullReferenceException("EndRequest method not found");
            }
        }

        internal static void Reset()
        {
            instance = null; // HACK?
        }

        /// <summary>
        /// Gets the singleton instance of the <see cref="GlimpseRuntime"/> type once it has been initialized
        /// </summary>
        public static GlimpseRuntime Instance
        {
            get
            {
                if (instance == null)
                {
                    throw new GlimpseNotInitializedException();
                }

                return instance;
            }

            private set { instance = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlimpseRuntime" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <exception cref="System.ArgumentNullException">Throws an exception if <paramref name="configuration"/> is <c>null</c>.</exception>
        public static void Initialize(IGlimpseConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            if (configuration.DefaultRuntimePolicy == RuntimePolicy.Off)
            {
                return;
            }

            var hasInited = false;
            if (!IsInitialized) // Double checked lock to ensure thread safety. http://en.wikipedia.org/wiki/Double_checked_locking_pattern
            {
                lock (LockObj)
                {
                    if (!IsInitialized)
                    {
                        Instance = new GlimpseRuntime(configuration);
                        hasInited = true;
                    }
                }
            }

            if (!hasInited && Instance.Configuration != configuration)
            {
                throw new NotSupportedException("Glimpse does not support being Initialized twice.");
            }
        }

        internal GlimpseRuntime(IGlimpseConfiguration configuration) // V2Merge: This should be private but is internal to not break unit tests
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            Configuration = configuration;
            this.Initialize();
        }

        /// <summary>
        /// Gets the executing version of Glimpse.
        /// </summary>
        /// <value>
        /// The version of Glimpse.
        /// </value>
        /// <remarks>Glimpse versioning follows the rules of <see href="http://semver.org/">Semantic Versioning</see>.</remarks>
        public static string Version { get; private set; }

        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public IGlimpseConfiguration Configuration { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance has been initialized.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is initialized; otherwise, <c>false</c>.
        /// </value>
        public static bool IsInitialized { get; private set; }

        /// <summary>
        /// Returns the <see cref="IGlimpseRequestContext"/> corresponding to the current request.
        /// </summary>
        public IGlimpseRequestContext CurrentRequestContext
        {
            get { return ActiveGlimpseRequestContexts.Current; }
        }

        private RuntimePolicyDeterminator RuntimePolicyDeterminator { get; set; }

        /// <summary>
        /// Returns the corresponding <see cref="IGlimpseRequestContext"/> for the given <paramref name="glimpseRequestId"/>
        /// </summary>
        /// <param name="glimpseRequestId">The Glimpse request Id</param>
        /// <param name="glimpseRequestContext">The corresponding <see cref="IGlimpseRequestContext"/></param>
        /// <returns>Boolean indicating whether the corresponding <see cref="IGlimpseRequestContext"/> was found.</returns>
        public bool TryGetRequestContext(Guid glimpseRequestId, out IGlimpseRequestContext glimpseRequestContext)
        {
            return ActiveGlimpseRequestContexts.TryGet(glimpseRequestId, out glimpseRequestContext);
        }

        private IDictionary<string, TabResult> GetTabResultsStore(IGlimpseRequestContext glimpseRequestContext)
        {
            return GetResultsStore<Dictionary<string, TabResult>>(glimpseRequestContext, Constants.TabResultsDataStoreKey);
        }

        private IDictionary<string, TabResult> GetDisplayResultsStore(IGlimpseRequestContext glimpseRequestContext)
        {
            return GetResultsStore<Dictionary<string, TabResult>>(glimpseRequestContext, Constants.DisplayResultsDataStoreKey);
        }

        private TResult GetResultsStore<TResult>(IGlimpseRequestContext glimpseRequestContext, string resultStoreKey)
            where TResult : class, new()
        {
            var requestStore = glimpseRequestContext.RequestStore;
            var resultStore = requestStore.Get<TResult>(resultStoreKey);

            if (resultStore == null)
            {
                resultStore = new TResult();
                requestStore.Set(resultStoreKey, resultStore);
            }

            return resultStore;
        }

        /// <summary>
        /// Begins Glimpse's processing of a Http request.
        /// </summary>
        /// <exception cref="Glimpse.Core.Framework.GlimpseException">Throws an exception if <see cref="GlimpseRuntime"/> is not yet initialized.</exception>
        public GlimpseRequestContextHandle BeginRequest(IRequestResponseAdapter requestResponseAdapter)
        {
            var glimpseRequestContext = new GlimpseRequestContext(Guid.NewGuid(), requestResponseAdapter, Configuration.DefaultRuntimePolicy, Configuration.EndpointBaseUri);

            var runtimePolicy = DetermineRuntimePolicy(RuntimeEvent.BeginRequest, glimpseRequestContext.CurrentRuntimePolicy, glimpseRequestContext.RequestResponseAdapter);
            if (runtimePolicy == RuntimePolicy.Off)
            {
                return UnavailableGlimpseRequestContextHandle.Instance;
            }

            glimpseRequestContext.CurrentRuntimePolicy = runtimePolicy;
            var glimpseRequestContextHandle = ActiveGlimpseRequestContexts.Add(glimpseRequestContext);

            if (glimpseRequestContextHandle.RequestHandlingMode == RequestHandlingMode.ResourceRequest)
            {
                // When we are dealing with a resource request, there is no need to further continue setting up the request.
                return glimpseRequestContextHandle;
            }

            try
            {
                glimpseRequestContext.StartTiming();

                ExecuteTabs(RuntimeEvent.BeginRequest, glimpseRequestContext);

                Configuration.MessageBroker.Publish(new RuntimeMessage()
                    .AsSourceMessage(typeof(GlimpseRuntime), MethodInfoBeginRequest)
                    .AsTimelineMessage("Start Request", TimelineCategory.Request)
                    .AsTimedMessage(glimpseRequestContext.CurrentExecutionTimer.Point()));

                return glimpseRequestContextHandle;
            }
            catch
            {
                // we need to deactivate here because the handle won't be returned to the caller
                glimpseRequestContextHandle.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Ends Glimpse's processing of the request referenced by the given <paramref name="glimpseRequestContextHandle"/>"/>
        /// </summary>
        /// <param name="glimpseRequestContextHandle">The Glimpse handle of the corresponding request</param>
        /// <exception cref="Glimpse.Core.Framework.GlimpseException">Throws an exception if <c>BeginRequest</c> has not yet been called for the given request.</exception>
        public void EndRequest(GlimpseRequestContextHandle glimpseRequestContextHandle) // TODO: Add PRG support
        {
            if (glimpseRequestContextHandle == null)
            {
                throw new ArgumentNullException("glimpseRequestContextHandle");
            }

            try
            {
                IGlimpseRequestContext glimpseRequestContext;
                if (!ContinueProcessingRequest(glimpseRequestContextHandle, RuntimeEvent.EndRequest, RequestHandlingMode.RegularRequest, out glimpseRequestContext))
                {
                    return;
                }

                Configuration.MessageBroker.Publish(new RuntimeMessage()
                    .AsSourceMessage(typeof(GlimpseRuntime), MethodInfoBeginRequest)
                    .AsTimelineMessage("End Request", TimelineCategory.Request)
                    .AsTimedMessage(glimpseRequestContext.CurrentExecutionTimer.Point()));

                ExecuteTabs(RuntimeEvent.EndRequest, glimpseRequestContext);
                ExecuteDisplays(glimpseRequestContext);

                TimeSpan timingDuration = glimpseRequestContext.StopTiming();

                var requestResponseAdapter = glimpseRequestContext.RequestResponseAdapter;
                var requestMetadata = requestResponseAdapter.RequestMetadata;
                var runtimePolicy = glimpseRequestContext.CurrentRuntimePolicy;

                if (runtimePolicy.HasFlag(RuntimePolicy.PersistResults))
                {
                    var persistenceStore = Configuration.PersistenceStore;

                    var metadata = new GlimpseRequest(
                        glimpseRequestContext.GlimpseRequestId,
                        requestMetadata,
                        GetTabResultsStore(glimpseRequestContext),
                        GetDisplayResultsStore(glimpseRequestContext),
                        timingDuration);

                    try
                    {
                        persistenceStore.Save(metadata);
                    }
                    catch (Exception exception)
                    {
                        Configuration.Logger.Error(Resources.GlimpseRuntimeEndRequesPersistError, exception, persistenceStore.GetType());
                    }
                }

                if (runtimePolicy.HasFlag(RuntimePolicy.ModifyResponseHeaders))
                {
                    requestResponseAdapter.SetHttpResponseHeader(Constants.HttpResponseHeader, glimpseRequestContext.GlimpseRequestId.ToString());

                    if (requestMetadata.GetCookie(Constants.ClientIdCookieName) == null)
                    {
                        requestResponseAdapter.SetCookie(Constants.ClientIdCookieName, requestMetadata.ClientId);
                    }
                }

                if (runtimePolicy.HasFlag(RuntimePolicy.DisplayGlimpseClient))
                {
                    var html = GenerateScriptTags(glimpseRequestContext);

                    requestResponseAdapter.InjectHttpResponseBody(html);
                }
            }
            finally
            {
                glimpseRequestContextHandle.Dispose();
            }
        }

        /// <summary>
        /// Begins access to session data.
        /// </summary>
        public void BeginSessionAccess(GlimpseRequestContextHandle glimpseRequestContextHandle)
        {
            IGlimpseRequestContext glimpseRequestContext;
            if (ContinueProcessingRequest(glimpseRequestContextHandle, RuntimeEvent.BeginSessionAccess, RequestHandlingMode.RegularRequest, out glimpseRequestContext))
            {
#warning should we add a try catch around this? So that failures in Glimpse don't fail the normal flow?
                ExecuteTabs(RuntimeEvent.BeginSessionAccess, glimpseRequestContext);
            }
        }

        /// <summary>
        /// Ends access to session data.
        /// </summary>
        public void EndSessionAccess(GlimpseRequestContextHandle glimpseRequestContextHandle)
        {
            IGlimpseRequestContext glimpseRequestContext;
            if (ContinueProcessingRequest(glimpseRequestContextHandle, RuntimeEvent.EndSessionAccess, RequestHandlingMode.RegularRequest, out glimpseRequestContext))
            {
#warning should we add a try catch around this? So that failures in Glimpse don't fail the normal flow?
                ExecuteTabs(RuntimeEvent.EndSessionAccess, glimpseRequestContext);
            }
        }

#warning CGI: There is no need to keep both execute methods, just have one default to default resource when resourcename is null
        /// <summary>
        /// Executes the default resource.
        /// </summary>
        public void ExecuteDefaultResource(GlimpseRequestContextHandle glimpseRequestContextHandle)
        {
            ExecuteResource(glimpseRequestContextHandle, Configuration.DefaultResource.Name, ResourceParameters.None());
        }

        /// <summary>
        /// Executes the given resource.
        /// </summary>
        /// <param name="glimpseRequestContextHandle">The Glimpse handle of the corresponding request</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">Throws an exception if either parameter is <c>null</c>.</exception>
        public void ExecuteResource(GlimpseRequestContextHandle glimpseRequestContextHandle, string resourceName, ResourceParameters parameters)
        {
            if (glimpseRequestContextHandle == null)
            {
                throw new ArgumentNullException("glimpseRequestContextHandle");
            }

            if (string.IsNullOrEmpty(resourceName))
            {
                throw new ArgumentNullException("resourceName");
            }

            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }

            IGlimpseRequestContext glimpseRequestContext;
            if (!ContinueProcessingRequest(glimpseRequestContextHandle, RuntimeEvent.ExecuteResource, RequestHandlingMode.ResourceRequest, out glimpseRequestContext))
            {
                return;
            }

            var requestResponseAdapter = glimpseRequestContext.RequestResponseAdapter;

            // First we get the current policy as it has been processed so far
            RuntimePolicy policy = glimpseRequestContext.CurrentRuntimePolicy;

            // It is possible that the policy now says Off, but if the requested resource is the default resource or one of it dependent resources, 
            // then we need to make sure there is a good reason for not executing that resource, since the default resource (or one of it dependencies)
            // is the one we most likely need to set Glimpse On with in the first place.
            IDependOnResources defaultResourceDependsOnResources = Configuration.DefaultResource as IDependOnResources;
            if (resourceName.Equals(Configuration.DefaultResource.Name) || (defaultResourceDependsOnResources != null && defaultResourceDependsOnResources.DependsOn(resourceName)))
            {
                // To be clear we only do this for the default resource (or its dependencies), and we do this because it allows us to secure the default resource 
                // the same way as any other resource, but for this we only rely on runtime policies that handle ExecuteResource runtime events and we ignore
                // ignore previously executed runtime policies (most likely during BeginRequest).
                // Either way, the default runtime policy is still our starting point and when it says Off, it remains Off
                policy = DetermineRuntimePolicy(RuntimeEvent.ExecuteResource, Configuration.DefaultRuntimePolicy, requestResponseAdapter);
            }

            string message;
            var logger = Configuration.Logger;
            var context = new ResourceResultContext(logger, requestResponseAdapter, Configuration.Serializer, Configuration.HtmlEncoder);

            if (policy == RuntimePolicy.Off)
            {
                string errorMessage = string.Format(Resources.ExecuteResourceInsufficientPolicy, resourceName);
                logger.Info(errorMessage);
                new StatusCodeResourceResult(403, errorMessage).Execute(context);
                return;
            }

            var resources =
                Configuration.Resources.Where(
                    r => r.Name.Equals(resourceName, StringComparison.InvariantCultureIgnoreCase));

            IResourceResult result;
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
                            result = privilegedResource.Execute(resourceContext, Configuration, requestResponseAdapter);
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
                    message = string.Format(Resources.ExecuteResourceMissingError, resourceName);
                    logger.Warn(message);
                    result = new StatusCodeResourceResult(404, message);
                    break;
                default: // 500 - Server Error
                    message = string.Format(Resources.ExecuteResourceDuplicateError, resourceName);
                    logger.Warn(message);
                    result = new StatusCodeResourceResult(500, message);
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

        private bool ContinueProcessingRequest(GlimpseRequestContextHandle glimpseRequestContextHandle, RuntimeEvent runtimeEvent, RequestHandlingMode allowedRequestHandlingMode, out IGlimpseRequestContext glimpseRequestContext)
        {
            glimpseRequestContext = null;

            if (glimpseRequestContextHandle == null)
            {
                throw new ArgumentNullException("glimpseRequestContextHandle");
            }

            if (glimpseRequestContextHandle.RequestHandlingMode != allowedRequestHandlingMode)
            {
                return false;
            }

            if (!TryGetRequestContext(glimpseRequestContextHandle.GlimpseRequestId, out glimpseRequestContext))
            {
#warning or maybe only a log and return false instead of throwing an exception? It is an isue though!
                throw new GlimpseException("No corresponding GlimpseRequestContext found for GlimpseRequestId '" + glimpseRequestContextHandle.GlimpseRequestId + "'.");
            }

            glimpseRequestContext.CurrentRuntimePolicy =
                DetermineRuntimePolicy(runtimeEvent, glimpseRequestContext.CurrentRuntimePolicy, glimpseRequestContext.RequestResponseAdapter);

            return glimpseRequestContext.CurrentRuntimePolicy != RuntimePolicy.Off;
        }

        /// <summary>
        /// Initializes this instance of the Glimpse runtime.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if system initialized successfully, <c>false</c> otherwise
        /// </returns>
        private void Initialize()
        {
            RuntimePolicyDeterminator = new RuntimePolicyDeterminator(Configuration.RuntimePolicies.ToArray(), Configuration.Logger);

            var logger = Configuration.Logger;
            var messageBroker = Configuration.MessageBroker;

            // TODO: Fix this to IDisplay no longer uses I*Tab*Setup
            var displaysThatRequireSetup = Configuration.Displays.Where(display => display is ITabSetup).Select(display => display);
            foreach (ITabSetup display in displaysThatRequireSetup)
            {
                var key = CreateKey(display);
                try
                {
                    var setupContext = new TabSetupContext(logger, messageBroker, () => GetTabStore(key, CurrentRequestContext));
                    display.Setup(setupContext);
                }
                catch (Exception exception)
                {
                    logger.Error(Resources.InitializeTabError, exception, key);
                }
            }

            var tabsThatRequireSetup = Configuration.Tabs.Where(tab => tab is ITabSetup).Select(tab => tab);
            foreach (ITabSetup tab in tabsThatRequireSetup)
            {
                var key = CreateKey(tab);
                try
                {
                    var setupContext = new TabSetupContext(logger, messageBroker, () => GetTabStore(key, CurrentRequestContext));
                    tab.Setup(setupContext);
                }
                catch (Exception exception)
                {
                    logger.Error(Resources.InitializeTabError, exception, key);
                }
            }

            var inspectorContext = new InspectorContext(
                logger,
                Configuration.ProxyFactory,
                messageBroker,
                () => ActiveGlimpseRequestContexts.Current.CurrentExecutionTimer,
                () => ActiveGlimpseRequestContexts.Current.CurrentRuntimePolicy);

            foreach (var inspector in Configuration.Inspectors)
            {
                try
                {
                    inspector.Setup(inspectorContext);
                    logger.Debug(Resources.GlimpseRuntimeInitializeSetupInspector, inspector.GetType());
                }
                catch (Exception exception)
                {
                    logger.Error(Resources.InitializeInspectorError, exception, inspector.GetType());
                }
            }

            PersistMetadata();
            IsInitialized = true;
        }

        private static string CreateKey(object obj)
        {
            string result;
            var keyProvider = obj as IKey;

            if (keyProvider != null)
            {
                result = keyProvider.Key;
            }
            else
            {
                result = obj.GetType().FullName;
            }

            return result
                .Replace('.', '_')
                .Replace(' ', '_')
                .ToLower();
        }

        private static IDataStore GetTabStore(string tabName, IGlimpseRequestContext glimpseRequestContext)
        {
            if (glimpseRequestContext.CurrentRuntimePolicy == RuntimePolicy.Off)
            {
                return null;
            }

            var requestStore = glimpseRequestContext.RequestStore;
            IDictionary<string, IDataStore> tabStorage;
            if (!requestStore.Contains(Constants.TabStorageKey))
            {
                tabStorage = new Dictionary<string, IDataStore>();
                requestStore.Set(Constants.TabStorageKey, tabStorage);
            }
            else
            {
                tabStorage = requestStore.Get<IDictionary<string, IDataStore>>(Constants.TabStorageKey);
            }

            IDataStore tabStore;
            if (!tabStorage.ContainsKey(tabName))
            {
                tabStore = new DictionaryDataStoreAdapter(new Dictionary<string, object>());
                tabStorage.Add(tabName, tabStore);
            }
            else
            {
                tabStore = tabStorage[tabName];
            }

            return tabStore;
        }

        private void ExecuteTabs(RuntimeEvent runtimeEvent, IGlimpseRequestContext glimpseRequestContext)
        {
            var runtimeContext = glimpseRequestContext.RequestResponseAdapter.RuntimeContext;
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
            var tabResultsStore = GetTabResultsStore(glimpseRequestContext);
            var logger = Configuration.Logger;

            foreach (var tab in supportedRuntimeTabs)
            {
                TabResult result;
                var key = CreateKey(tab);
                try
                {
                    var tabContext = new TabContext(runtimeContext, GetTabStore(key, glimpseRequestContext), logger, messageBroker);
                    var tabData = tab.GetData(tabContext);

                    var tabSection = tabData as TabSection;
                    if (tabSection != null)
                    {
                        tabData = tabSection.Build();
                    }

                    result = new TabResult(tab.Name, tabData);
                }
                catch (Exception exception)
                {
                    result = new TabResult(tab.Name, exception.ToString());
                    logger.Error(Resources.ExecuteTabError, exception, key);
                }

                if (tabResultsStore.ContainsKey(key))
                {
                    tabResultsStore[key] = result;
                }
                else
                {
                    tabResultsStore.Add(key, result);
                }
            }
        }

        private void ExecuteDisplays(IGlimpseRequestContext glimpseRequestContext)
        {
            var runtimeContext = glimpseRequestContext.RequestResponseAdapter.RuntimeContext;
            var messageBroker = Configuration.MessageBroker;

            var displayResultsStore = GetDisplayResultsStore(glimpseRequestContext);
            var logger = Configuration.Logger;

            foreach (var display in Configuration.Displays)
            {
                TabResult result; // TODO: Rename now that it is no longer *just* tab results
                var key = CreateKey(display);
                try
                {
                    var displayContext = new TabContext(runtimeContext, GetTabStore(key, glimpseRequestContext), logger, messageBroker); // TODO: Do we need a DisplayContext?
                    var displayData = display.GetData(displayContext);

                    result = new TabResult(display.Name, displayData);
                }
                catch (Exception exception)
                {
                    result = new TabResult(display.Name, exception.ToString());
                    logger.Error(Resources.ExecuteTabError, exception, key);
                }

                if (displayResultsStore.ContainsKey(key))
                {
                    displayResultsStore[key] = result;
                }
                else
                {
                    displayResultsStore.Add(key, result);
                }
            }
        }

        private void PersistMetadata()
        {
            var metadata = new GlimpseMetadata { Version = Version, Hash = Configuration.Hash };
            var tabMetadata = metadata.Tabs;

            foreach (var tab in Configuration.Tabs)
            {
                var metadataInstance = new TabMetadata();

                var documentationTab = tab as IDocumentation;
                if (documentationTab != null)
                {
                    metadataInstance.DocumentationUri = documentationTab.DocumentationUri;
                }

                var layoutControlTab = tab as ILayoutControl;
                if (layoutControlTab != null)
                {
                    metadataInstance.KeysHeadings = layoutControlTab.KeysHeadings;
                }

                var layoutTab = tab as ITabLayout;
                if (layoutTab != null)
                {
                    metadataInstance.Layout = layoutTab.GetLayout();
                }

                if (metadataInstance.HasMetadata)
                {
                    tabMetadata[CreateKey(tab)] = metadataInstance;
                }
            }

            var resources = metadata.Resources;
            var endpoint = Configuration.ResourceEndpoint;
            var logger = Configuration.Logger;

            foreach (var resource in Configuration.Resources)
            {
                var resourceKey = CreateKey(resource);
                if (resources.ContainsKey(resourceKey))
                {
                    logger.Warn(Resources.GlimpseRuntimePersistMetadataMultipleResourceWarning, resource.Name);
                }

                resources[resourceKey] = endpoint.GenerateUriTemplate(resource, Configuration.EndpointBaseUri, logger);
            }

            Configuration.PersistenceStore.Save(metadata);
        }

        // TODO this should not be public! This was changed to hack in OWIN support
        public string GenerateScriptTags(GlimpseRequestContextHandle glimpseRequestContextHandle)
        {
            if (glimpseRequestContextHandle == null)
            {
                throw new ArgumentNullException("glimpseRequestContextHandle");
            }

            if (glimpseRequestContextHandle.RequestHandlingMode != RequestHandlingMode.RegularRequest)
            {
                return string.Empty;
            }

            IGlimpseRequestContext glimpseRequestContext;
            if (!TryGetRequestContext(glimpseRequestContextHandle.GlimpseRequestId, out glimpseRequestContext))
            {
                throw new GlimpseException("No corresponding GlimpseRequestContext found for GlimpseRequestId '" + glimpseRequestContextHandle.GlimpseRequestId + "'.");
            }

            return GenerateScriptTags(glimpseRequestContext);
        }

#warning this should not be public! but we need to have some way to get to generate script tags conditionally so that they are only generated once (like glimpse injects it before </body> and at the same time a user has added the GlimpseClient control)
        public string GenerateScriptTags(IGlimpseRequestContext glimpseRequestContext)
        {
            if (glimpseRequestContext.CurrentRuntimePolicy == RuntimePolicy.Off)
            {
                return string.Empty;
            }

            var requestStore = glimpseRequestContext.RequestStore;
            var hasRendered = false;

            if (requestStore.Contains(Constants.ScriptsHaveRenderedKey))
            {
                hasRendered = requestStore.Get<bool>(Constants.ScriptsHaveRenderedKey);
            }

            if (hasRendered)
            {
                return string.Empty;
            }

            var glimpseScriptTags = GlimpseScriptTagsGenerator.Generate(glimpseRequestContext.GlimpseRequestId, Configuration, Version);

            requestStore.Set(Constants.ScriptsHaveRenderedKey, true);
            return glimpseScriptTags;
        }

        private RuntimePolicy DetermineRuntimePolicy(RuntimeEvent runtimeEvent, RuntimePolicy currentRuntimePolicy, IRequestResponseAdapter requestResponseAdapter)
        {
            var runtimePolicyResult = RuntimePolicyDeterminator.DetermineRuntimePolicy(runtimeEvent, currentRuntimePolicy, requestResponseAdapter);

            if (runtimePolicyResult.Messages.Length != 0)
            {
                string allMessages = runtimePolicyResult.Messages[0].Message; 
                if(runtimePolicyResult.Messages.Length > 1)
                {
                    allMessages = runtimePolicyResult.Messages.Aggregate("RuntimePolicy determination messages :", (concatenatedMessages, message) => concatenatedMessages += Environment.NewLine + "\t" + message.Message);
                }

                if (runtimePolicyResult.Messages.Any(message => message.IsWarning))
                {
                    Configuration.Logger.Warn(allMessages);
                }
                else
                {
                    Configuration.Logger.Debug(allMessages);
                }
            }

            return runtimePolicyResult.RuntimePolicy;
        }

        /// <summary>
        /// The message used to to track the beginning and end of Http requests.
        /// </summary>
        protected class RuntimeMessage : ITimelineMessage, ISourceMessage
        {
            /// <summary>
            /// Gets the id of the request.
            /// </summary>
            /// <value>
            /// The id.
            /// </value>
            public Guid Id { get; private set; }

            /// <summary>
            /// Gets or sets the name of the event.
            /// </summary>
            /// <value>
            /// The name of the event.
            /// </value>
            public string EventName { get; set; }

            /// <summary>
            /// Gets or sets the event category.
            /// </summary>
            /// <value>
            /// The event category.
            /// </value>
            public TimelineCategoryItem EventCategory { get; set; }

            /// <summary>
            /// Gets or sets the event sub text.
            /// </summary>
            /// <value>
            /// The event sub text.
            /// </value>
            public string EventSubText { get; set; }

            /// <summary>
            /// Gets or sets the type of the executed.
            /// </summary>
            /// <value>
            /// The type of the executed.
            /// </value>
            public Type ExecutedType { get; set; }

            /// <summary>
            /// Gets or sets the executed method.
            /// </summary>
            /// <value>
            /// The executed method.
            /// </value>
            public MethodInfo ExecutedMethod { get; set; }

            /// <summary>
            /// Gets or sets the offset.
            /// </summary>
            /// <value>
            /// The offset.
            /// </value>
            public TimeSpan Offset { get; set; }

            /// <summary>
            /// Gets or sets the duration.
            /// </summary>
            /// <value>
            /// The duration.
            /// </value>
            public TimeSpan Duration { get; set; }

            /// <summary>
            /// Gets or sets the start time.
            /// </summary>
            /// <value>
            /// The start time.
            /// </value>
            public DateTime StartTime { get; set; }
        }
    }
}