using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Glimpse.Core.Logging
{
    public class GlimpseLoggerFactory
    { 
        private LogFactory _factory;
        private bool _loggingEnabled;

        public GlimpseLoggerFactory(bool loggingEnabled)
        {
            _loggingEnabled = loggingEnabled;
            if (_loggingEnabled)
                _factory = BuildFactory(); 
        }

        public Logger CreateLogger(string name)
        {
            if (_loggingEnabled)
                return _factory.GetLogger(name);
            return LogManager.CreateNullLogger();
        }
         
        private LogFactory BuildFactory()
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
