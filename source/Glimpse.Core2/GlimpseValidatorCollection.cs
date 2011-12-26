using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2
{
    public class GlimpseValidatorCollection:GlimpseCollection<IGlimpseValidator>
    {
        public GlimpseMode GetMode(RequestMetadata requestMetadata)
        {
            var result = GlimpseMode.On;
            foreach (var validator in this)
            {
                var mode = validator.GetMode(requestMetadata); //TODO: Exception handling
                if (mode < result)
                    result = mode;

                if (result == GlimpseMode.Off)
                    break;
            }

            return result;
        }
    }
}