using System;
using System.Text;
using Glimpse.Core.Framework;

namespace Glimpse.Core
{
    public class GlimpseScriptsInjectionOptions
    {
        private IGlimpseScriptTagsProvider ScriptTagsProvider { get; set; }
        private Func<Encoding> ContentEncodingProvider { get; set; }

        private bool? _injectionRequired;
        private Encoding _contentEncoding;
        private string _scriptTags;

        public event EventHandler<GlimpseScriptsInjectionFailedEventArgs> InjectionFailed = delegate { };

        public GlimpseScriptsInjectionOptions(
            IGlimpseScriptTagsProvider scriptTagsProvider,
            Func<Encoding> contentEncodingProvider,
            EventHandler<GlimpseScriptsInjectionFailedEventArgs> onInjectionFailed = null)
        {
            Guard.ArgumentNotNull("scriptTagsProvider", scriptTagsProvider);
            Guard.ArgumentNotNull("contentEncodingProvider", contentEncodingProvider);

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

        public string ScriptTags
        {
            get { return _scriptTags ?? (_scriptTags = ScriptTagsProvider.GetScriptTags()); }
        }

        public Encoding ContentEncoding
        {
            get { return _contentEncoding ?? (_contentEncoding = ContentEncodingProvider()); }
        }

        public void NotifyInjectionFailure(string failureMessage)
        {
            InjectionFailed(this, new GlimpseScriptsInjectionFailedEventArgs(failureMessage));
        }
    }
}