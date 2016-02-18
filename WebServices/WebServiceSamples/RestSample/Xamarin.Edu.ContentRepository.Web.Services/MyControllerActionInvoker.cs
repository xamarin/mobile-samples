using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;

namespace Xamarin.Edu.ContentRepository.Web.Services
{
	public class MyControllerActionInvoker : ControllerActionInvoker
	{
		/// <summary>
		/// Overriding FindAction to determine is a generic parameter exists, then use custom ActionDescriptor
		/// </summary>
		/// <param name="controllerContext"></param>
		/// <param name="controllerDescriptor"></param>
		/// <param name="actionName"></param>
		/// <returns></returns>
		protected override ActionDescriptor FindAction(ControllerContext controllerContext, ControllerDescriptor controllerDescriptor, string actionName)
		{
			ActionDescriptor actionDescriptor = null;
			MethodInfo mi = controllerContext.Controller.GetType().GetMethods().SingleOrDefault(x => x.Name.ToLower() == actionName.ToLower());

			if(mi != null)
			{
				if(mi.ContainsGenericParameters)
				{
					actionDescriptor = new MyActionDescriptor(mi, actionName, controllerDescriptor);
				}
			}
			if(actionDescriptor == null)
			{
				actionDescriptor = base.FindAction(controllerContext, controllerDescriptor, actionName);
			}
			return actionDescriptor;
		}
	}
}