using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Glimpse.Owin.Middleware
{
    public class MiddlewareManager
    {
        private const string trackerKey = "glimpse.MiddlewareTracker";
        private static MiddlewareManager instance;
        private readonly IDictionary<Guid, List<Type>> registeredMiddleware; 

        private MiddlewareManager()
        {
            registeredMiddleware = new Dictionary<Guid, List<Type>>();
        }

        public static MiddlewareManager Instance 
        {
            get { return instance ?? (instance = new MiddlewareManager()); }
        }

        public void Register(Guid builderId, Type middlewareType)
        {
            List<Type> chain;
            if (registeredMiddleware.ContainsKey(builderId))
            {
                chain = registeredMiddleware[builderId];
            }
            else
            {
                registeredMiddleware[builderId] = chain = new List<Type>();
            }

            chain.Add(middlewareType);
        }

        public void Start(IDictionary<string, object> environment, Type middlewareType, Guid builderId)
        {
            var tracker = GetTracker(environment);

            tracker.Push(MiddlewareExecutionInfo.Running(middlewareType));
        }

        public void End(IDictionary<string, object> environment, Type middlewareType, Guid builderId)
        {
            var tracker = GetTracker(environment);
            var middleware = tracker.Pop();
            middleware.Stop();

            // add in missing parts of chain
            if (middleware.Children.Count == 0)
            {
                var chain = registeredMiddleware[builderId];
                var child = middleware;
                foreach (var registrant in chain.SkipWhile(m => m != middlewareType).Skip(1))
                {
                    var newChild = MiddlewareExecutionInfo.Unrun(registrant);
                    child.Children.Add(newChild);
                    child = newChild;
                }
            }
        }

        private MiddlewareTracker GetTracker(IDictionary<string, object> environment)
        {
            if (environment.ContainsKey(trackerKey) && environment[trackerKey] is MiddlewareTracker)
            {
                return (MiddlewareTracker)environment[trackerKey];
            }

            var result = new MiddlewareTracker();
            environment[trackerKey] = result;
            return result;
        }
    }

    public class MiddlewareTracker
    {
        public MiddlewareTracker()
        {
            Stack = new Stack<MiddlewareExecutionInfo>();
        }

        public MiddlewareExecutionInfo Graph { get; set; }

        public Stack<MiddlewareExecutionInfo> Stack { get; set; }

        public void Push(MiddlewareExecutionInfo executionInfo)
        {
            if (Graph == null)
            {
                Graph = executionInfo;
            }
            else
            {
                Graph.Children.Add(executionInfo);
            }

            Stack.Push(executionInfo);
        }

        public MiddlewareExecutionInfo Pop()
        {
            return Stack.Pop();
        }
    }
}