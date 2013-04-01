using System.Collections.Generic; 
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework.Support
{
    public abstract class ExecutionBlockBase
    {
        private readonly object hasInitalizedLock = new object();
        private bool hasInitalized;
        private ILogger logger;
        private IList<IExecutionTask> tasks = new List<IExecutionTask>();

        private ILogger Logger
        {
            get
            {
                if (logger == null)
                {
                    logger = GlimpseConfiguration.GetLogger() ?? new NullLogger();
                }

                return logger;
            }
        }

        /// <summary>
        /// Allows tasks to be added which will be executed when the Block
        /// is executed.
        /// </summary>
        /// <param name="task">Task to be executed</param>
        public void RegisterProvider(IExecutionTask task)
        {
            tasks.Add(task);
        }

        /// <summary>
        /// Execute any 
        /// </summary>
        public void Execute()
        {
            if (!hasInitalized)
            {
                lock (hasInitalizedLock)
                {
                    if (!hasInitalized)
                    {
                        foreach (var task in tasks)
                        {
                            task.Execute(Logger);
                        }

                        hasInitalized = true;
                    }
                }
            }
        }
    }

}
