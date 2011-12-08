using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NerdDinner
{
	public class MobileCapableWebFormViewEngine : WebFormViewEngine
	{
		public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
		{
			ViewEngineResult result = null;
			var request = controllerContext.HttpContext.Request;

			//This could be replaced with a switch statement as other advanced / device specific views are created
			if (UserAgentIs(controllerContext, "iPhone"))	{
				result = base.FindView(controllerContext, "Mobile/iPhone/" + viewName, masterName, useCache);
			}

			// Avoid unnecessary checks if this device isn't suspected to be a mobile device
			if (request.Browser.IsMobileDevice)
			{
				//TODO: We are not doing any thing WinMobile SPECIAL yet!

				//if (UserAgentIs(controllerContext, "MSIEMobile 6"))	{
				//  result = base.FindView(controllerContext, "Mobile/MobileIE6/" + viewName, masterName, useCache);
				//}
				//else if (UserAgentIs(controllerContext, "PocketIE") && request.Browser.MajorVersion >= 4)
				//{
				//  result = base.FindView(controllerContext, "Mobile/PocketIE/" + viewName, masterName, useCache);
				//}

				//Fall back to default mobile view if no other mobile view has already been set
				if ((result == null || result.View == null) &&
								request.Browser.IsMobileDevice)
				{
					result = base.FindView(controllerContext, "Mobile/" + viewName, masterName, useCache);
				}
			}

			//Fall back to desktop view if no other view has been selected
			if (result == null || result.View == null)
			{
				result = base.FindView(controllerContext, viewName, masterName, useCache);
			}

			return result;
		}

		public bool UserAgentIs(ControllerContext controllerContext, string userAgentToTest)
		{
			return (controllerContext.HttpContext.Request.UserAgent.IndexOf(userAgentToTest,
							StringComparison.OrdinalIgnoreCase) > 0);
		}
	}
}