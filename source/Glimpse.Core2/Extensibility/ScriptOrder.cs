namespace Glimpse.Core2.Extensibility
{
    public enum ScriptOrder
    {
        IncludeBeforeClientInterfaceScript,
        ClientInterfaceScript,
        IncludeAfterClientInterfaceScript,
        IncludeBeforeRequestDataScript,
        RequestDataScript, 
        RequestMetadataScript,
        IncludeAfterRequestDataScript,
    }
}