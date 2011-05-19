namespace Glimpse.WebForms.Extensibility
{
    public interface IGlimpsePluginRequirements
    {
        bool SessionRequired { get; }
        bool ShouldSetupInInit { get; }
    }
}
