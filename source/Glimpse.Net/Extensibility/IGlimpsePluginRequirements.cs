namespace Glimpse.Net.Extensibility
{
    public interface IGlimpsePluginRequirements
    {
        bool SessionRequired { get; }
        bool ShouldSetupInInit { get; }
    }
}
