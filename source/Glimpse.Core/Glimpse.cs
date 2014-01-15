using System;
using Glimpse.Core.Framework;

namespace Glimpse.Core
{
    public static class Glimpse
    {
        private static Func<IGlimpseConfiguration, IGlimpseConfiguration> configuration = config => config;

        public static Func<IGlimpseConfiguration, IGlimpseConfiguration> Configuration
        {
            get { return configuration; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Configration");
                }

                configuration = value;
            }
        }
    }
}
