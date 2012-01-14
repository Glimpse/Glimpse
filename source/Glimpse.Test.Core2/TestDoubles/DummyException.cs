using System;

namespace Glimpse.Test.Core2.TestDoubles
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