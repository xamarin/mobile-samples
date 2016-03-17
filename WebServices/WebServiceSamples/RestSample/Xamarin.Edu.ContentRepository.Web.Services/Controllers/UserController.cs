using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using M=Xamarin.Edu.ContentRepository.Web.Services.Models;
using Xamarin.Edu.ContentRepository.Web.Services.Helpers;

namespace Xamarin.Edu.ContentRepository.Web.Services.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/

        public T GetUser<T>(int id)
        {
			// since this is just a sample, we won't bother to pull from a data source.
			M.User user = new M.User()
			{
				 FirstName = "FirstName" + Environment.TickCount,
				 LastName = "LastName" + Environment.TickCount,
				 EmailAddress = "EmailAddress" + Environment.TickCount,
			};
			return ControllerHelper.GetReturnObject<T>(user);
        }

    }
}
