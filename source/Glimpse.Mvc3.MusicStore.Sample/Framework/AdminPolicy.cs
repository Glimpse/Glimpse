//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using Glimpse.AspNet.Extensions;
//using Glimpse.Core.Extensibility;

//namespace MvcMusicStore.Framework
//{
//    public class AdminPolicy : IRuntimePolicy
//    {
//        public RuntimePolicy Execute(IRuntimePolicyContext policyContext)
//        {
//            var httpContext = policyContext.GetHttpContext();
//            if (!httpContext.User.IsInRole("Administrator"))
//            {
//                return RuntimePolicy.Off;
//            }

//            return RuntimePolicy.On;
//        }

//        public RuntimeEvent ExecuteOn
//        {
//            get { return RuntimeEvent.EndRequest; }
//        }
//    }
//}