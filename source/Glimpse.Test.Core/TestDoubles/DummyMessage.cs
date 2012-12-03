using Glimpse.Core.Extensibility;
using Glimpse.Core.Message;

namespace Glimpse.Test.Core.TestDoubles
{
    public class DummyMessage : MessageBase, IDummyInterface
    {
        public string Identifier { get; set; }

        public string Name { get; set; }
    }
}