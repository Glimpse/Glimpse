using Glimpse.EF.Inspector.Core;

namespace Glimpse.EF
{
    public static class Initialize
    {
        public static void Start()
        {
            EntityFrameworkExecutionBlock.Instance.Execute();
        }
    }
}
