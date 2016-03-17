using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Xamarin.Edu.ContentRepository.Web.Services
{
	public class JsonpResult : JsonResult
	{
		const string DefaultCallback = "Callback";
		const string JsonContentType = "application/json";

		public override void ExecuteResult(ControllerContext context)
		{
			if(context == null)
			{
				throw new ArgumentNullException("context");
			}

			HttpResponseBase response = context.HttpContext.Response;

			// assign the content type
			if(!String.IsNullOrEmpty(ContentType))
			{
				response.ContentType = ContentType;
			}
			else
			{
				response.ContentType = JsonContentType;
			}

			// assign content encoding
			if(ContentEncoding != null)
			{
				response.ContentEncoding = ContentEncoding;
			}

			// assign the data
			if(Data != null)
			{
				HttpRequestBase request = context.HttpContext.Request;

				// assign the call back
				string callback = DefaultCallback;
				if(request.Params["callback"] != null)
				{
					callback = request.Params["callback"].ToString();
				}
				string json = Helpers.Serializer.Serialize(Data, Helpers.Serializer.SerializationType.Jsonp, callback);
				response.Write(json);
			}
		}
	}
}