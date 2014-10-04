using System;
using Xamarin.UITest;
using Xamarin.UITest.Queries;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.UITest.Android;

namespace Tasky.UITest
{
    public static  class Helpers
    {
        public static void ClearText(this IApp app, string id)
        {
            if (app is AndroidApp)
            {
                app.Query(c => c.Id(id).Invoke("setText", string.Empty));
            }
            else
            {
                throw new NotImplementedException("Don't know how to clear text on " + id + ".");
            }
        }

        public static void ClearAndSetText(this IApp app, string id, string newText)
        {
            app.ClearText(id);
            app.Query(c => c.Id(id).Invoke("setText", newText));
        }
    }
}

