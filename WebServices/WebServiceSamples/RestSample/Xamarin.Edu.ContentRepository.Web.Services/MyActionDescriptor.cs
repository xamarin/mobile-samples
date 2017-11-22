using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;

namespace Xamarin.Edu.ContentRepository.Web.Services
{
	/// <summary>
	/// Customer ActionDescriptor to pull the format from the querystring and inject the appropriate return type for the Action
	/// </summary>
	public class MyActionDescriptor : ActionDescriptor
	{
		private string _actionName;
		private MethodInfo _mi;
		private ControllerDescriptor _controllerDescriptor;

		public MyActionDescriptor( MethodInfo mi, string actionName, ControllerDescriptor controllerDescriptor )
		{
			if( mi == null )
			{
				throw new ArgumentNullException( "methodInfo" );
			}
			if( string.IsNullOrEmpty( actionName ) )
			{
				throw new ArgumentNullException( "actionName" );
			}
			if( controllerDescriptor == null )
			{
				throw new ArgumentNullException( "controllerDescriptor" );
			}

			this._actionName = actionName;
			this._mi = mi;
			this._controllerDescriptor = controllerDescriptor;
		}

		public override string ActionName
		{
			get { return this._actionName; }
		}

		public override ControllerDescriptor ControllerDescriptor
		{
			get { return this._controllerDescriptor; }
		}

		public override object Execute( ControllerContext controllerContext, IDictionary<string, object> parameters )
		{
			if( controllerContext == null )
			{
				throw new ArgumentNullException( "controllerContext" );
			}
			if( parameters == null )
			{
				throw new ArgumentNullException( "parameters" );
			}

			ParameterInfo[] parameterInfos = _mi.GetParameters();
			List<object> list = new List<object>();

			foreach( ParameterInfo pi in parameterInfos )
			{
				if( parameters.Keys.Contains( pi.Name ) )
				{
					list.Add( parameters[pi.Name] );
				}
			}

			// This is where we read in the "format" querystring parameter and set the return type of either xml (default), json, or jsonp
			Type returnType = typeof( XmlResult );
			if( controllerContext.HttpContext.Request.Params["format"] != null )
			{
				string format = controllerContext.HttpContext.Request.Params["format"].ToString().Trim().ToLower();
				switch( format )
				{
					case "json":
						returnType = typeof( JsonResult );
						break;
					case "jsonp":
						returnType = typeof( JsonpResult );
						break;
					case "xml":
					default: // xml is default
						returnType = typeof( XmlResult );
						break;
				}
			}

			// here were are creating the custom return type
			MethodInfo genericMethod = _mi.MakeGenericMethod( returnType );
			object actionReturnValue = genericMethod.Invoke( controllerContext.Controller, list.ToArray() );
			return actionReturnValue;
		}

		public override ParameterDescriptor[] GetParameters()
		{
			ParameterInfo[] parameterInfos = _mi.GetParameters();
			List<ParameterDescriptor> list = new List<ParameterDescriptor>();
			foreach( ParameterInfo pi in parameterInfos )
			{
				list.Add( new ReflectedParameterDescriptor( pi, this ) );
			}
			return list.ToArray();
		}
	}
}