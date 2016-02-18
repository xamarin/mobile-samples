using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Xamarin.Edu.ContentRepository.Web.Services
{
	public class MyControllerFactory : DefaultControllerFactory
	{
		/// <summary>
		/// Overriding CreateController to use custom Action Invoker
		/// </summary>
		/// <param name="requestContext"></param>
		/// <param name="controllerName"></param>
		/// <returns></returns>
		public override IController CreateController(System.Web.Routing.RequestContext requestContext, string controllerName)
		{
			IController controller = base.CreateController(requestContext, controllerName);
			((Controller)controller).ActionInvoker = new MyControllerActionInvoker();
			return controller;
		}

	}
}