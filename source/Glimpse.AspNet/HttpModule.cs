using System;
using System.Threading;
using System.Web;
using System.Web.Compilation;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.AspNet
{
    public class HttpModule : IHttpModule
    {
        private static readonly object glimpseRuntimeWrapperInitializationLock = new object();
        private static readonly Factory Factory;

        static HttpModule()
        {
            var serviceLocator = new AspNetServiceLocator();
            Factory = new Factory(serviceLocator);
            ILogger logger = Factory.InstantiateLogger();
            serviceLocator.Logger = logger;

            try
            {
                BuildManager.GetReferencedAssemblies();
                serviceLocator.Logger.Debug("Preloaded all referenced assemblies with System.Web.Compilation.BuildManager.GetReferencedAssemblies()");
            }
            catch (Exception exception)
            {
                serviceLocator.Logger.Error("Call to System.Web.Compilation.BuildManager.GetReferencedAssemblies() failed.", exception);
            }

            AppDomain.CurrentDomain.SetData(Constants.LoggerKey, logger);
            AppDomain.CurrentDomain.DomainUnload += (sender, e) => OnAppDomainUnload((AppDomain)sender);
        }

        private static void OnAppDomainUnload(AppDomain appDomain)
        {
            ILogger logger = GetCurrentLogger();

            logger.Fatal(
                "AppDomain with Id: '{0}' and BaseDirectory: '{1}' has been unloaded. Any in memory data stores have been lost. {2}",
                appDomain.Id,
                appDomain.BaseDirectory,
                HttpRuntimeShutdownMessageResolver.ResolveShutdownMessage());

            // NLog writes its logs asynchronously, which means that if we don't wait, chances are the log will not be written 
            // before the appdomain is actually shut down, so we sleep for 100ms and hopefully that is enough for NLog to do its thing
            Thread.Sleep(100);
        }

        public void Init(HttpApplication httpApplication)
        {
            GetCurrentLogger().Debug(
                Resources.HttpModuleInitIsCalled,
                this.GetType(),
                this.GetHashCode(),
                httpApplication.GetType(),
                httpApplication.GetHashCode());

            Init(new HttpApplicationWrapper(httpApplication));
        }

        public void Dispose()
        {
            // Nothing to dispose
        }

        internal void Init(HttpApplicationBase httpApplication)
        {
            var glimpseRuntimeWrapper = this.GetGlimpseRuntimeWrapper(httpApplication.Application);
            glimpseRuntimeWrapper.Initialize(httpApplication);
        }

        internal GlimpseRuntimeWrapper GetGlimpseRuntimeWrapper(HttpApplicationStateBase applicationState)
        {
            Func<GlimpseRuntimeWrapper> getStoredGlimpseRuntimeWrapper = () => applicationState[Constants.RuntimeKey] as GlimpseRuntimeWrapper;

            var glimpseRuntimeWrapper = getStoredGlimpseRuntimeWrapper();

            if (glimpseRuntimeWrapper == null)
            {
                lock (glimpseRuntimeWrapperInitializationLock)
                {
                    glimpseRuntimeWrapper = getStoredGlimpseRuntimeWrapper();

                    if (glimpseRuntimeWrapper == null)
                    {
                        ILogger logger = GetCurrentLogger();
                        glimpseRuntimeWrapper = new GlimpseRuntimeWrapper(Factory.InstantiateFrameworkProvider(), Factory.InstantiateRuntime(), logger, Factory.GetLoggingConfiguration().WriteRequestFlowHandlingLogs);
                        applicationState.Add(Constants.RuntimeKey, glimpseRuntimeWrapper);

                        logger.Debug(
                            Resources.HttpModuleInstantiatedGlimpseRuntimeWrapper,
                            glimpseRuntimeWrapper.GetType(),
                            applicationState.GetType(),
                            applicationState.GetHashCode());
                    }
                }
            }

            return glimpseRuntimeWrapper;
        }

        private static ILogger GetCurrentLogger()
        {
            ILogger logger = AppDomain.CurrentDomain.GetData(Constants.LoggerKey) as ILogger;
            if (logger == null)
            {
                throw new GlimpseException("There is no '" + typeof(ILogger).FullName + "' available in the current AppDomain's data, behind the key '" + Constants.LoggerKey + "'.");
            }

            return logger;
        }
    }
}
