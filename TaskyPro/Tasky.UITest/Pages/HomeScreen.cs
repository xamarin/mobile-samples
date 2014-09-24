using System;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Tasky.UITest
{

    public class HomeScreen: Page
    {

        public static TaskDetailsScreen NavigateToTaskDetails(IApp app)
        {
            var home = new HomeScreen();
            return home.NavigateTo<TaskDetailsScreen>(home.ToTaskDetails(app));
        }

        public HomeScreen()
        {
            Marked = new ScreenQuery("action_bar_title", c => c.Id("action_bar_title").Text("Tasky Pro"));
            AddTaskButton = new ScreenQuery("add_task_button", c => c.Id("menu_add_task"));
        }

        public ScreenQuery AddTaskButton { get; private set; }


        public Func<IApp> ToTaskDetails(IApp app)
        {
            return () =>
            {
                app.Tap(AddTaskButton);
                return app;
            };
        }

        public int NumberOfRows(IApp app)
        {
            return app.Query(c => c.Id("linearLayout1")).Length;
        }

        public bool HasOneTaskNamed(IApp app, string name)
        {
            var result = app.Query(c => c.Id("linearLayout1").Descendant(null).Id("lblName").Text(name));
            return result.Length == 1;
        }

        public void ViewTaskNamed(IApp app, string taskName)
        {
            NavigateTo<TaskDetailsScreen>(() =>
            {
                app.Tap(c => c.Id("linearLayout1").Descendant(null).Id("lblName").Text(taskName));
                return app;
            });
        }
    }
    
}
