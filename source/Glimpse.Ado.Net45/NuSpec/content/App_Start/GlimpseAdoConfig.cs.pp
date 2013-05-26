[assembly: WebActivator.PreApplicationStartMethod(typeof($rootnamespace$.GlimpseAdoConfig), "Start")]

namespace $rootnamespace$
{
    using Glimpse;
    using Glimpse.Ado;

    public static class GlimpseAdoConfig 
    {
        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            Settings.Initialize.Ado();
        }
    }
}

