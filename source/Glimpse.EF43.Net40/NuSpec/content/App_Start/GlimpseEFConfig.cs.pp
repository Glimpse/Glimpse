[assembly: WebActivator.PreApplicationStartMethod(typeof($rootnamespace$.GlimpseEFConfig), "Start")]

namespace $rootnamespace$
{
    using Glimpse;
    using Glimpse.EF;

    public static class GlimpseEFConfig 
    {
        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            Settings.Initialize.EF();
        }
    }
}

