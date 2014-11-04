namespace Glimpse.Core.Framework
{
    public interface IScriptTagsProvider
    {
        bool ScriptTagsAllowedToBeProvided { get; }
        bool ScriptTagsAlreadyProvided { get; }
        string GetScriptTags();
    }
}