using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNet.Abstractions;

namespace Glimpse.ProjectK.Middleware
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

        public void Start(HttpContext context, Type middlewareType, Guid builderId)
        {
            var tracker = GetTracker(context);

            tracker.Push(MiddlewareExecutionInfo.Running(middlewareType));
        }

        public void End(HttpContext context, Type middlewareType, Guid builderId)
        {
            var tracker = GetTracker(context);
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

        private MiddlewareTracker GetTracker(HttpContext context)
        {
            var items = context.Items;
            if (items.ContainsKey(trackerKey) && items[trackerKey] is MiddlewareTracker)
            {
                return (MiddlewareTracker)items[trackerKey];
            }

            var result = new MiddlewareTracker();
            items[trackerKey] = result;
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