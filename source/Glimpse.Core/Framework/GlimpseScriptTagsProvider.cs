using System;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    public class GlimpseScriptTagsProvider : IGlimpseScriptTagsProvider
    {
        private Guid GlimpseRequestId { get; set; }
        private IGlimpseScriptTagsGenerator GlimpseScriptTagsGenerator { get; set; }
        private ILogger Logger { get; set; }
        private bool ScriptTagsAlreadyProvided { get; set; }
        private Func<bool> IsAllowedToProvideScriptTags { get; set; }

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

        public string DetermineScriptTags()
        {
            try
            {
                return ScriptTagsAlreadyProvided ? string.Empty : GenerateScriptTags();
            }
            finally
            {
                ScriptTagsAlreadyProvided = true;
            }
        }

        private string GenerateScriptTags()
        {
            if (CanScriptTagsBeProvided())
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

        private bool CanScriptTagsBeProvided()
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