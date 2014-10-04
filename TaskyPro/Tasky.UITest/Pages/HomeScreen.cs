using System;
using Xamarin.UITest;
using Xamarin.UITest.Queries;
using System.Security.Cryptography;
using Xamarin.UITest.Android;

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

        public bool HasOneTaskNamed(IApp app, string name, string notes = null)
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

        public void UpdateTaskNamed(IApp app, string taskName, string newName, string newNotes)
        {
            var details = NavigateTo<TaskDetailsScreen>(() =>
            {
                app.Tap(c => c.Id("linearLayout1").Descendant(null).Id("lblName").Text(taskName));
                return app;
            });

            app.WaitForElement(details.Marked);

            app.Screenshot("The original task.");


            var result = app.Query(c=>c.Id("txtName").Invoke("setText", newName));
            var r2 = app.Query(c => c.Id("txtNotes").Invoke("setText", newNotes));

            app.Screenshot("The updated task.");

            app.Tap(details.SaveButton);

            app.WaitForElement(Marked);
        }


    }
    
}
