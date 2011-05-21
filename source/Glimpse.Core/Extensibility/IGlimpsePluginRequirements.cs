namespace Glimpse.Core.Extensibility
{
    public interface IGlimpsePluginRequirements
    {
        bool SessionRequired { get; }
        bool ShouldSetupInInit { get; }
    }
}
