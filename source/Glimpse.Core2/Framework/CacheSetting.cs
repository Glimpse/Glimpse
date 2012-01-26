using System.ComponentModel;

namespace Glimpse.Core2.Framework
{
    public enum CacheSetting
    {
        [Description("public")]
        Public,
        [Description("private")]
        Private,
        [Description("no-cache")]
        NoCache,
        [Description("no-store")]
        NoStore,
        [Description("must-revalidate")]
        MustRevalidate,
        [Description("proxy-revalidate")]
        ProxyRevalidate
    }
}