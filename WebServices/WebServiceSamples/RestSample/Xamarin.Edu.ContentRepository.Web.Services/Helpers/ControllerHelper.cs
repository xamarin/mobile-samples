using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Xamarin.Edu.ContentRepository.Web.Services.Helpers
{
	/// <summary>
	/// Class to help with Controller level methods
	/// </summary>
	public static class ControllerHelper
	{
		/// <summary>
		/// Handle returning the correct Type based on the template parameter.  If the type is ot Json, Jsonp, or XML, we'll handle 
		/// it with the corresponding result type,  Otherwise, return with a cast.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static T GetReturnObject<T>(object obj)
		{
			// return standard JsonResult
			if(typeof(T) == typeof(JsonResult))
			{
				object result = new JsonResult();
				((JsonResult)result).JsonRequestBehavior = JsonRequestBehavior.AllowGet;
				((JsonResult)result).Data = obj;
				return (T)result;
			}
			// return JsonpResult (assign the data here and we'll handle the callback wrapping in the serializer)
			else if(typeof(T) == typeof(JsonpResult))
			{
				object result = new JsonpResult();
				((JsonpResult)result).Data = obj;
				return (T)result;
			}
			// return XmlResult (assign the data here and we'll handle the rest in the serializer)
			else if(typeof(T) == typeof(XmlResult))
			{
				object result = new XmlResult();
				((XmlResult)result).Data = obj;
				return (T)result;
			}
			// otherwise, let's cast and return it
			else
			{
				return (T)obj;
			}
		}
	}
}