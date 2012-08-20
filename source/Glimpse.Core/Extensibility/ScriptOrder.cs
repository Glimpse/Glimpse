namespace Glimpse.Core.Extensibility
{
    public enum ScriptOrder
    {
        IncludeBeforeClientInterfaceScript,
        ClientInterfaceScript,
        IncludeAfterClientInterfaceScript,
        RequestMetadataScript,
        IncludeBeforeRequestDataScript,
        RequestDataScript, 
        IncludeAfterRequestDataScript,
    }
}