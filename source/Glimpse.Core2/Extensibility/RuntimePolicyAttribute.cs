using System;
using System.ComponentModel.Composition;

namespace Glimpse.Core2.Extensibility
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class RuntimePolicyAttribute:ExportAttribute, IRuntimePolicyMetadata
    {
         public RuntimePolicyAttribute():base(typeof(IRuntimePolicy))
         {
             RuntimeEvent = null;
         }
        
        public RuntimePolicyAttribute(RuntimeEvent runtimeEvent):base(typeof(IRuntimePolicy))
        {
            RuntimeEvent = runtimeEvent;
        }

         public RuntimeEvent? RuntimeEvent { get; set; }
    }
}