using System;
using Xamarin.UITest;
using Xamarin.UITest.Queries;
using System.Linq;
using System.Threading.Tasks;

namespace Tasky.UITest
{
    public static  class Helpers
    {

        public static void ClearText(this IApp app, string id)
        {
            var result = app.Query(("setText", string.Empty));
        }
    }
}

