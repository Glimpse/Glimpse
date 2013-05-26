using Glimpse.Ado.Inspector.Core;

namespace Glimpse.Ado
{
    public static class Initialize
    {
        public static void Start()
        {
            AdoExecutionBlock.Instance.Execute();
        }

        /// <remarks>For use with .NET 3.5</remarks>
        public static void Ado(this Glimpse.Core.Setting.Initializer initializer)
        {
            Start();
        }
    }
}
