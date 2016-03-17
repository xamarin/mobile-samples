using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Xamarin.Edu.ContentRepository.Web.Services.Helpers;

namespace Xamarin.Edu.ContentRepository.Web.Services
{
	/// <summary>
	/// XmlResult class to return xml as an action result.
	/// </summary>
	public class XmlResult : ActionResult
	{
		public string ContentType { get; set; }
		public Encoding ContentEncoding { get; set; }
		public object Data { get; set; }

		public XmlResult()
		{
			this.ContentType = "application/xml";
			this.ContentEncoding = Encoding.Default;
			this.Data = null;
		}

		/// <summary>
		/// Execute the Xml result.
		/// </summary>
		/// <param name="context"></param>
		public override void ExecuteResult(ControllerContext context)
		{
			if(context == null)
			{
				throw new ArgumentNullException("context");
			}

			HttpResponseBase response = context.HttpContext.Response;
			response.ContentType = ContentType;
			response.ContentEncoding = ContentEncoding;

			// ---- Serialize the data
			if(Data != null)
			{
				HttpRequestBase request = context.HttpContext.Request;
				response.Write(Serializer.Serialize(Data, Serializer.SerializationType.XML));
			}

		}
	}
}