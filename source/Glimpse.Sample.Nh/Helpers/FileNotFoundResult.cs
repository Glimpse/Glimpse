using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NerdDinner.Helpers
{
    public class FileNotFoundResult : ActionResult
    {
        public string Message { 
            get; 
            set; 
        }

        public override void ExecuteResult(ControllerContext context) {
            throw new HttpException(404, Message);
        }
    }
}
