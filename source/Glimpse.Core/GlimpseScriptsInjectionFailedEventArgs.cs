using System;

namespace Glimpse.Core
{
    public class GlimpseScriptsInjectionFailedEventArgs : EventArgs
    {
        public GlimpseScriptsInjectionFailedEventArgs(string failureMessage)
        {
            FailureMessage = failureMessage;
        }

        public string FailureMessage { get; private set; }
    }
}