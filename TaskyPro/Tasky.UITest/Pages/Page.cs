using System;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Tasky.UITest
{
    public class Page
    {
        /// <summary>
        /// Used to identify some view that is unique to this screen.
        /// </summary>
        /// <value>The marked.</value>
        public ScreenQuery Marked { get; protected set; }

        public T NavigateTo<T>(Func<IApp> nextPage) where T:Page, new()
        {
            var app = nextPage();
            var target = Activator.CreateInstance<T>();
            var msg = string.Format("Timed out waiting for the view '{0}' on class {1}.", target.Marked.Name, target.GetType().Name);
            var results = app.WaitForElement(target.Marked, msg);

            if (results.Any() && target.IsDisplayed(app))
            {
                return (T)target;
            }
            else
            {
                return null;
            }
        }

        public bool IsDisplayed(IApp app)
        {
            return app.Query(Marked).Any();
        }

    }
    
}
