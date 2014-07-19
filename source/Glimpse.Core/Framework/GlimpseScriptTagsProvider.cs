using System;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    public class GlimpseScriptTagsProvider : IGlimpseScriptTagsProvider
    {
        private Guid GlimpseRequestId { get; set; }
        private IGlimpseScriptTagsGenerator GlimpseScriptTagsGenerator { get; set; }
        private ILogger Logger { get; set; }

        private Func<bool> IsAllowedToProvideScriptTags { get; set; }

        private bool? _scriptTagsAllowedToBeProvided;

        public GlimpseScriptTagsProvider(
            Guid glimpseRequestId,
            IGlimpseScriptTagsGenerator glimpseScriptTagsGenerator,
            ILogger logger,
            Func<bool> isAllowedToProvideScriptTags)
        {
            Guard.ArgumentNotNull("glimpseScriptTagsGenerator", glimpseScriptTagsGenerator);
            Guard.ArgumentNotNull("logger", logger);
            Guard.ArgumentNotNull("isAllowedToProvideScriptTags", isAllowedToProvideScriptTags);

            GlimpseRequestId = glimpseRequestId;
            GlimpseScriptTagsGenerator = glimpseScriptTagsGenerator;
            Logger = logger;
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
                    return GlimpseScriptTagsGenerator.Generate(GlimpseRequestId);
                }
                catch (Exception exception)
                {
                    Logger.Error("Failed to generate script tags", exception);
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
                Logger.Error("Failed to determine whether script tags are allowed to be provided.", exception);
                return false;
            }
        }
    }
}