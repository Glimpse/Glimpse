namespace Glimpse.Core.Framework
{
    public interface IGlimpseScriptTagsProvider
    {
        bool ScriptTagsAllowedToBeProvided { get; }
        bool ScriptTagsAlreadyProvided { get; }
        string GetScriptTags();
    }
}