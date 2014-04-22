using System;
using Glimpse.Core.Configuration;

namespace Glimpse.Core
{
    public static class GlimpseConfiguration
    {
        private static Action<IConfiguration> @override = delegate { };

        public static Action<IConfiguration> Override
        {
            get
            {
                return @override;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                @override = value;
            }
        }
    }
}