using System;

namespace Glimpse.Core.Framework
{
    public class GlimpseScriptTagsProvider : IGlimpseScriptTagsProvider
    {
        private Guid GlimpseRequestId { get; set; }
        private IGlimpseScriptTagsGenerator ScriptTagsGenerator { get; set; }
        private Action<string, Exception> OnExceptionCallback { get; set; }

        private Func<bool> IsAllowedToProvideScriptTags { get; set; }

        private bool? _scriptTagsAllowedToBeProvided;

        public GlimpseScriptTagsProvider(
            Guid glimpseRequestId,
            IGlimpseScriptTagsGenerator scriptTagsGenerator,
            Func<bool> isAllowedToProvideScriptTags,
            Action<string, Exception> onExceptionCallback = null)
        {
            Guard.ArgumentNotNull("scriptTagsGenerator", scriptTagsGenerator);
            Guard.ArgumentNotNull("isAllowedToProvideScriptTags", isAllowedToProvideScriptTags);

            GlimpseRequestId = glimpseRequestId;
            ScriptTagsGenerator = scriptTagsGenerator;
            OnExceptionCallback = onExceptionCallback ?? delegate { };
            IsAllowedToProvideScriptTags = isAllowedToProvideScriptTags;
        }

        public bool ScriptTagsAllowedToBeProvided
        {
            get { return (_scriptTagsAllowedToBeProvided ?? (_scriptTagsAllowedToBeProvided = DetermineIfScriptTagsAreAllowedToBeProvided())).Value; }
        }

        public bool ScriptTagsAlreadyProvided { get; private set; }

        public string GetScriptTags()
        {
            try
            {
                return ScriptTagsAlreadyProvided ? string.Empty : ProvideScriptTags();
            }
            finally
            {
                ScriptTagsAlreadyProvided = true;
            }
        }

        private string ProvideScriptTags()
        {
            if (ScriptTagsAllowedToBeProvided)
            {
                try
                {
                    return ScriptTagsGenerator.Generate(GlimpseRequestId);
                }
                catch (Exception exception)
                {
                    OnExceptionCallback("Failed to generate script tags", exception);
                    return string.Empty;
                }
            }

            return string.Empty;
        }

        private bool DetermineIfScriptTagsAreAllowedToBeProvided()
        {
            try
            {
                return IsAllowedToProvideScriptTags();
            }
            catch (Exception exception)
            {
                OnExceptionCallback("Failed to determine whether script tags are allowed to be provided.", exception);
                return false;
            }
        }
    }
}