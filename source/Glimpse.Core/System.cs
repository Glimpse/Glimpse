using Glimpse.Core.Setting;

namespace Glimpse
{
    /// <summary>
    /// Hook that is used to hang any system settings 
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// Hook that is used to hang initialization code from. No method 
        /// here is required to be called. You should only need to use this 
        /// code if Glimpse doesn't initialize soon enough in the application
        /// life cycle for your purposes.
        /// </summary>
        public readonly static Initializer Initialize = new Initializer();
    }
}
