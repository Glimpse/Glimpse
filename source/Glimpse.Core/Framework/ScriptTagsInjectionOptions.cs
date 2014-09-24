using System;
using System.Text;

namespace Glimpse.Core.Framework
{
    public class ScriptTagsInjectionOptions
    {
        private IScriptTagsProvider ScriptTagsProvider { get; set; }
        private Func<Encoding> ContentEncodingProvider { get; set; }

        private bool? _injectionRequired;
        private Encoding _contentEncoding;

        public event EventHandler<ScriptTagsInjectionFailedEventArgs> InjectionFailed = delegate { };

        public ScriptTagsInjectionOptions(
            IScriptTagsProvider scriptTagsProvider,
            Func<Encoding> contentEncodingProvider,
            EventHandler<ScriptTagsInjectionFailedEventArgs> onInjectionFailed = null)
        {
            Guard.ArgumentNotNull(scriptTagsProvider, "scriptTagsProvider");
            Guard.ArgumentNotNull(contentEncodingProvider, "contentEncodingProvider");

            ScriptTagsProvider = scriptTagsProvider;
            ContentEncodingProvider = contentEncodingProvider;

            if (onInjectionFailed != null)
            {
                InjectionFailed += onInjectionFailed;
            }
        }

        public bool InjectionRequired
        {
            get { return (_injectionRequired ?? (_injectionRequired = ScriptTagsProvider.ScriptTagsAllowedToBeProvided)).Value; }
        }

        public string GetScriptTagsToInject()
        {
            return ScriptTagsProvider.GetScriptTags();
        }

        public Encoding ContentEncoding
        {
            get { return _contentEncoding ?? (_contentEncoding = ContentEncodingProvider()); }
        }

        public void NotifyInjectionFailure(string failureMessage)
        {
            InjectionFailed(this, new ScriptTagsInjectionFailedEventArgs(failureMessage));
        }
    }
}