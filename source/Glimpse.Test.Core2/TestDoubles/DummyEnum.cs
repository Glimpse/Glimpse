using System.ComponentModel;

namespace Glimpse.Test.Core2.TestDoubles
{
    public enum DummyEnum
    {
        [Description("I am described")]
        WithDescription,
        WithoutDescription
    }
}