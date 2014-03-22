using System;
using Glimpse.Core.Framework;

namespace Glimpse.Core
{
    public static class GlimpseConfiguration
    {
        private static Func<IConfiguration, IConfiguration> @override = config => config;

        public static Func<IConfiguration, IConfiguration> Override
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
