using Glimpse.Core.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Glimpse.Core.Extensibility
{
    public class StackTraceFilter
    {
        private readonly HashSet<string> _excludedTypes = new System.Collections.Generic.HashSet<string>();
        private readonly HashSet<string> _excludedMethods = new System.Collections.Generic.HashSet<string>();
        private readonly HashSet<string> _excludedAssemblies = new System.Collections.Generic.HashSet<string>();

        private IEnumerable<StackFrameInfo> GetFilteredMethods(StackTrace trace)
        {
            if (trace == null)
            {
                yield break;
            }

            foreach (var frame in trace.GetFrames())
            {
                var method = frame.GetMethod();

                if (ShouldExcludeMethod(method) ||
                    ShouldExcludeType(method) ||
                    ShouldExcludeAssembly(method.Module.Assembly))
                {
                    continue;
                }

                yield return new StackFrameInfo(frame, method);
            }
        }

        public string GetFilteredStackTrace(StackTrace trace)
        {
            var methodNames =
                (from method in GetFilteredMethods(trace)
                 select method.ToString()).ToArray();

            return string.Join(Environment.NewLine, methodNames);
        }

        protected virtual bool ShouldExcludeMethod(MethodBase method)
        {
            return _excludedMethods.Contains(method.Name);
        }

        protected virtual bool ShouldExcludeType(MethodBase method)
        {
            var t = method.DeclaringType;

            while (t != null)
            {
                if (_excludedTypes.Contains(t.Name))
                {
                    return true;
                }

                t = t.DeclaringType;
            }

            return false;
        }

        protected virtual bool ShouldExcludeAssembly(Assembly assembly)
        {
            return _excludedAssemblies.Contains(assembly.GetName().Name);
        }

        public void ExcludeType(string type)
        {
            _excludedTypes.Add(type);
        }

        public void ExcludeMethod(string methodName)
        {
            _excludedMethods.Add(methodName);
        }

        public void ExcludeAssembly(string assemblyName)
        {
            _excludedAssemblies.Add(assemblyName);
        }

        private class StackFrameInfo
        {
            private const string AnonymousMethodDescription = " --- () => {...}";

            private readonly StackFrame _frame;
            private readonly MethodBase _method;

            public StackFrameInfo(StackFrame frame, MethodBase method)
            {
                _frame = frame;
                _method = method;
            }

            private bool IsAnonymousMethod
            {
                get
                {
                    // Don't know if this will work in all cases...
                    return _frame.ToString().StartsWith("<") ||
                        _method.Name.Equals("lambda_method", StringComparison.OrdinalIgnoreCase);
                }
            }

            private string FullMethodName
            {
                get
                {
                    string methodName = string.Format("{0}()", _method.Name);
                    if (_method.DeclaringType == null)
                    {
                        return methodName;

                    }
                    return string.Format("{0}.{1}", _method.DeclaringType.FullName, methodName);
                }
            }

            public override string ToString()
            {
                try
                {
                    return IsAnonymousMethod ? string.Concat(FullMethodName, AnonymousMethodDescription) : FullMethodName;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    throw e;
                }
            }
        }
    }

    public class ReflectionBlackListStackFrameFilter : StackTraceFilter
    {
        protected override bool ShouldExcludeMethod(MethodBase method)
        {
            return false;
        }

        protected override bool ShouldExcludeType(MethodBase method)
        {
            return false;
        }

        protected override bool ShouldExcludeAssembly(Assembly assembly)
        {
            if (assembly.GetName().Name.ToUpperInvariant().StartsWith("GLIMPSE"))
            {
                return true;
            }

            return ReflectionBlackList.IsBlackListed(assembly);
        }
    }
}
