using System.ComponentModel;

namespace Glimpse.Test.Core.TestDoubles
{
    public enum DummyEnum
    {
        [Description("I am described")]
        WithDescription,
        WithoutDescription
    }
}