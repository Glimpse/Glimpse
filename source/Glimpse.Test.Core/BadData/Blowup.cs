using Glimpse.Test.Core.TestDoubles;

namespace Glimpse.Test.Core.BadData
{
    public class Blowup:IBlowup
    {
        public Blowup()
        {
            throw new DummyException("I blow up on purpose.");
        }

        public string AProperty
        {
            get { throw new DummyException("I blow up on purpose."); }
        }
    }
}