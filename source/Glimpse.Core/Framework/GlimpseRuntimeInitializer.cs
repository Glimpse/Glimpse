using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    public partial class GlimpseRuntime
    {
        private static readonly object initializationLock = new object();

        /// <summary>
        /// Initializer used by the <see cref="GlimpseRuntime"/> to initialize the Glimpse runtime
        /// </summary>
        public class GlimpseRuntimeInitializer
        {
            private IList<InitializationMessage> InitializationMessages { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="GlimpseRuntimeInitializer" />
            /// </summary>
            internal GlimpseRuntimeInitializer()
            {
                InitializationMessages = new List<InitializationMessage>();
            }

            /// <summary>
            /// Allows the Glimpse runtime host to add additional initialization messages that will be written to the 
            /// Glimpse log once the Glimpse logger is available.
            /// </summary>
            /// <param name="level">The <see cref="LoggingLevel"/> of the message</param>
            /// <param name="message">The message</param>
            /// <param name="exception">A possible exception related to the message</param>
            public void AddInitializationMessage(LoggingLevel level, string message, Exception exception = null)
            {
                InitializationMessages.Add(new InitializationMessage
                {
                    Level = level,
                    Message = message,
                    Exception = exception
                });
            }

            /// <summary>
            /// Initializes the Glimpse runtime by finalizing the <paramref name="configuration"/> and creating a new
            /// <see cref="GlimpseRuntime"/> that will be set as the <see cref="GlimpseRuntime.Instance"/>
            /// </summary>
            /// <param name="configuration">The configuration used to initialize the Glimpse runtime</param>
            public void Initialize(IConfiguration configuration)
            {
                Guard.ArgumentNotNull("configuration", configuration);

                lock (initializationLock)
                {
                    // we always take a lock as concurrent initialization calls should not happen, but if they do, they'll have to wait on each other
                    if (!IsAvailable)
                    {
                        // Run user customizations to configuration before storing and then override 
                        // (some) changes made by the user to make sure .config file driven settings win
                        var userUpdatedConfig = GlimpseConfiguration.Override(configuration);
                        userUpdatedConfig.ApplyOverrides();

                        if (configuration.DefaultRuntimePolicy == RuntimePolicy.Off)
                        {
                            return;
                        }

                        // now that the Logger is available, we can log the registered initialization messages
                        foreach (var initializationMessage in InitializationMessages.Where(initializationMessage => !initializationMessage.WrittenToLog))
                        {
                            switch (initializationMessage.Level)
                            {
                                case LoggingLevel.Trace:
                                    configuration.Logger.Trace(initializationMessage.Message, initializationMessage.Exception);
                                    break;
                                case LoggingLevel.Debug:
                                    configuration.Logger.Debug(initializationMessage.Message, initializationMessage.Exception);
                                    break;
                                case LoggingLevel.Info:
                                    configuration.Logger.Info(initializationMessage.Message, initializationMessage.Exception);
                                    break;
                                case LoggingLevel.Warn:
                                    configuration.Logger.Warn(initializationMessage.Message, initializationMessage.Exception);
                                    break;
                                case LoggingLevel.Error:
                                    configuration.Logger.Error(initializationMessage.Message, initializationMessage.Exception);
                                    break;
                                case LoggingLevel.Fatal:
                                    configuration.Logger.Fatal(initializationMessage.Message, initializationMessage.Exception);
                                    break;
                            }

                            initializationMessage.WrittenToLog = true;
                        }

                        var readonlyConfiguration = new ReadonlyConfigurationAdapter(userUpdatedConfig);

                        var activeGlimpseRequestContexts = new ActiveGlimpseRequestContexts(readonlyConfiguration.CurrentGlimpseRequestIdTracker);

                        var displayProvider = new DisplayProvider(readonlyConfiguration, activeGlimpseRequestContexts);
                        displayProvider.Setup();

                        var tabProvider = new TabProvider(readonlyConfiguration, activeGlimpseRequestContexts);
                        tabProvider.Setup();

                        var inspectorProvider = new InspectorProvider(readonlyConfiguration, activeGlimpseRequestContexts);
                        inspectorProvider.Setup();

                        var metadataProvider = new MetadataProvider(readonlyConfiguration);
                        metadataProvider.SaveMetadata();

                        var runtimePolicyDeterminator = new RuntimePolicyDeterminator(readonlyConfiguration);

                        GlimpseRuntime.Instance = new GlimpseRuntime(
                            readonlyConfiguration,
                            activeGlimpseRequestContexts,
                            runtimePolicyDeterminator,
                            metadataProvider,
                            tabProvider,
                            displayProvider);
                    }
                }
            }

            private class InitializationMessage
            {
                public LoggingLevel Level { get; set; }
                
                public string Message { get; set; }
                
                public Exception Exception { get; set; }
                
                public bool WrittenToLog { get; set; }
            }
        }
    }
}