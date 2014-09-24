using System;

namespace Glimpse.Core.Framework
{
    public class ScriptTagsInjectionFailedEventArgs : EventArgs
    {
        public ScriptTagsInjectionFailedEventArgs(string failureMessage)
        {
            FailureMessage = failureMessage;
        }

        public string FailureMessage { get; private set; }
    }
}