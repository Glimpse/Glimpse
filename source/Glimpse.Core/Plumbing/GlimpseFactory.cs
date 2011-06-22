using System.Diagnostics;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Glimpse.Core.Plumbing
{
    public class GlimpseFactory:IGlimpseFactory
    {
        internal LogFactory Factory { get; set; }
        internal GlimpseConfiguration Configuration { get; set; }

        public GlimpseFactory(GlimpseConfiguration configuration)
        {
            Configuration = configuration;
            if (Configuration.LoggingEnabled) Factory = BuildFactory(); 
        }

        public IGlimpseLogger CreateLogger()
        {
            if (!Configuration.LoggingEnabled) return new GlimpseLogger(LogManager.CreateNullLogger());

            var stackTrace = new StackTrace();
            var frame = stackTrace.GetFrame(1);
            var name = frame.GetMethod().DeclaringType.FullName;

            return new GlimpseLogger(Factory.GetLogger(name));
        }
         
        private static LogFactory BuildFactory()
        {
            // Step 1. Create configuration object 
            var config = new LoggingConfiguration();

            // Step 2. Create targets and add them to the configuration  
            var fileTarget = new FileTarget();
            config.AddTarget("file", fileTarget);

            // Step 3. Set target properties  
            fileTarget.FileName = "${basedir}/Glimpse.log";
            fileTarget.Layout = "${longdate}|${level:uppercase=true}|${logger}|${message}";

            // Step 4. Define rules 
            var rule2 = new LoggingRule("*", LogLevel.Debug, fileTarget);
            config.LoggingRules.Add(rule2);
             
            return new LogFactory(config);  
        }
    }
}
