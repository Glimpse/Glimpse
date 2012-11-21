using Glimpse.Core.Extensibility;

namespace Glimpse.Test.Core.TestDoubles
{
    public class DummyMessage : MessageBase, IDummyInterface
    {
        public string Identifier { get; set; }

        public string Name { get; set; }
    }
}