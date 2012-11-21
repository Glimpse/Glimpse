namespace Glimpse.AspNet.Model
{
    public class EnvironmentTimeZoneModel
    {
        public string Name { get; set; }

        public bool IsDaylightSavingTime { get; set; }

        public int UtcOffset { get; set; }

        public int UtcOffsetWithDls { get; set; }
    }
}