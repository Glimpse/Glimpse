namespace Glimpse.Protocol
{
    public interface IGlimpsePluginRequirements
    {
        bool SessionRequired { get; }
        bool ShouldSetupInInit { get; }
    }
}
