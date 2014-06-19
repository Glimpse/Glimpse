using System;
using System.Diagnostics;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// Represents an unavailable <see cref="IGlimpseRequestContext"/>
    /// </summary>
    public class UnavailableGlimpseRequestContext : IGlimpseRequestContext
    {
        private Guid glimpseRequestId;
        private IDataStore requestStore;
        private IExecutionTimer currentExecutionTimer;

        /// <summary>
        /// Gets the singleton instance of the <see cref="UnavailableGlimpseRequestContext"/> type.
        /// </summary>
        public static UnavailableGlimpseRequestContext Instance { get; private set; }

        static UnavailableGlimpseRequestContext()
        {
            Instance = new UnavailableGlimpseRequestContext();
        }

        private UnavailableGlimpseRequestContext()
        {
            GlimpseRequestId = new Guid();
            RequestStore = new DataStoreStub();
            CurrentExecutionTimer = new ExecutionTimerStub();
            ScriptTagsProvider = new GlimpseScriptTagsProviderStub();
        }

        /// <summary>
        /// Gets a default <see cref="Guid"/> representing the unavailable request
        /// </summary>
        public Guid GlimpseRequestId
        {
            get { return LogAccess("GlimpseRequestId", () => glimpseRequestId); }
            private set { glimpseRequestId = value; }
        }

        /// <summary>
        /// Gets the <see cref="IRequestResponseAdapter"/> of the unavailable request
        /// </summary>
        public IRequestResponseAdapter RequestResponseAdapter
        {
            get { return LogAccess<IRequestResponseAdapter>("RequestResponseAdapter", () => null); }
        }

        /// <summary>
        /// Gets the <see cref="IDataStore"/> for the unavailable request
        /// </summary>
        public IDataStore RequestStore
        {
            get { return LogAccess("RequestStore", () => requestStore); }
            private set { requestStore = value; }
        }

        /// <summary>
        /// Gets the active <see cref="RuntimePolicy"/> for the unavailable request
        /// </summary>
        public RuntimePolicy CurrentRuntimePolicy
        {
            get
            {
                return RuntimePolicy.Off;
            }

            set
            {
                if (value > RuntimePolicy.Off)
                {
                    throw new GlimpseException("You're not allowed to increase the active runtime policy level from 'RuntimePolicy.Off' to '" + value + "'.");
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="RequestHandlingMode"/> for the unavailable request
        /// </summary>
        public RequestHandlingMode RequestHandlingMode
        {
            get { return RequestHandlingMode.Unhandled; }
        }

        /// <summary>
        /// Gets the <see cref="GlimpseScriptTagsProvider"/> for the referenced request
        /// </summary>
        public IGlimpseScriptTagsProvider ScriptTagsProvider { get; private set; }

        /// <summary>
        /// Gets the <see cref="IExecutionTimer"/> for the referenced request
        /// </summary>
        public IExecutionTimer CurrentExecutionTimer
        {
            get { return LogAccess("CurrentExecutionTimer", () => currentExecutionTimer); }
            private set { currentExecutionTimer = value; }
        }

        /// <summary>
        /// Starts timing the execution of the referenced request
        /// </summary>
        public void StartTiming()
        {
            LogAccess("StartTiming");
        }

        /// <summary>
        /// Stops timing the execution of the referenced request
        /// </summary>
        /// <returns>The elapsed time since the start of the timing</returns>
        public TimeSpan StopTiming()
        {
            return LogAccess("StopTiming", () => TimeSpan.Zero);
        }

        private static T LogAccess<T>(string propertyOrMethodName, Func<T> propertyValueProvider)
        {
            if (GlimpseRuntime.IsAvailable)
            {
                GlimpseRuntime.Instance.Configuration.Logger.Warn("Accessing 'UnavailableGlimpseRequestContext.Instance." + propertyOrMethodName + "' which is unexpected. Make sure to check the current runtime policy before accessing any further details. StackTrace = " + new StackTrace());
            }

            return propertyValueProvider();
        }

        private static void LogAccess(string methodName)
        {
            LogAccess(methodName, () => true);
        }

        private class DataStoreStub : IDataStore
        {
            public object Get(string key)
            {
                LogAccess("RequestStore.Get");
                return null;
            }

            public void Set(string key, object value)
            {
                LogAccess("RequestStore.Set");
            }

            public bool Contains(string key)
            {
                LogAccess("RequestStore.Contains");
                return false;
            }
        }

        private class ExecutionTimerStub : IExecutionTimer
        {
            public DateTime RequestStart
            {
                get { return LogAccess("CurrentExecutionTimer.RequestStart", () => DateTime.MinValue); }
            }

            public TimerResult Point()
            {
                return LogAccess("CurrentExecutionTimer.Point", () => new TimerResult { Duration = TimeSpan.Zero, Offset = TimeSpan.Zero, StartTime = DateTime.MinValue });
            }

            public TimerResult<T> Time<T>(Func<T> function)
            {
                return LogAccess("CurrentExecutionTimer.Time<T>", () => new TimerResult<T> { Duration = TimeSpan.Zero, Offset = TimeSpan.Zero, StartTime = DateTime.MinValue, Result = function() });
            }

            public TimerResult Time(Action action)
            {
                action();
                return LogAccess("CurrentExecutionTimer.Time", () => new TimerResult { Duration = TimeSpan.Zero, Offset = TimeSpan.Zero, StartTime = DateTime.MinValue });
            }

            public TimeSpan Start()
            {
                return LogAccess("CurrentExecutionTimer.Start", () => TimeSpan.Zero);
            }

            public TimerResult Stop(TimeSpan offset)
            {
                return LogAccess("CurrentExecutionTimer.Stop", () => new TimerResult { Duration = TimeSpan.Zero, Offset = TimeSpan.Zero, StartTime = DateTime.MinValue });
            }
        }

        private class GlimpseScriptTagsProviderStub : IGlimpseScriptTagsProvider
        {
            public string DetermineScriptTags()
            {
                return string.Empty;
            }
        }
    }
}