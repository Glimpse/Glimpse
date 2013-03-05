using System.Web.Mvc;

namespace Glimpse.Test.Mvc.TestDoubles
{
    public class DummyAsyncController : AsyncController
    {
         public void IndexAsync()
         {
             AsyncManager.OutstandingOperations.Increment();
             AsyncManager.OutstandingOperations.Decrement();
         }

        public string IndexCompleted()
        {
            return "This was async";
        }
    }
}