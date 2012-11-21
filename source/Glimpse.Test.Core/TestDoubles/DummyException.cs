using System;

namespace Glimpse.Test.Core.TestDoubles
{
    public class DummyException:Exception
    {
        public DummyException():base()
        {
            
        }

        public DummyException(string message):base(message)
        {
            
        }
    }
}