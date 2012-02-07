using System;
using Glimpse.Test.Core2.TestDoubles;

namespace Glimpse.Test.Core2.BadData
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