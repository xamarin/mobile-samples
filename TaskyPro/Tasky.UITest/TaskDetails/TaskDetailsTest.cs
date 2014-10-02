using System;
using NUnit.Framework;

namespace Tasky.UITest
{
    [TestFixture]
    public abstract class TaskDetailsTest: TaskyProTest
    {
        public HomeScreen Home { get; protected set; }

        public TaskDetailsScreen TaskDetails { get; protected set; }

        public override void TestFixtureSetup()
        {
            base.TestFixtureSetup();
            Home = new HomeScreen();
            TaskDetails = new TaskDetailsScreen();
        }

        protected void WaitForHomeScreen()
        {
            App.WaitForElement(Home.Marked);
            Assert.That(Home.IsDisplayed(App), Is.True, "Should have returned to the Home Screen");
        }

        protected void AssertHomeScreenShowsTasks(int count)
        {
            WaitForHomeScreen();
            var result = App.Query(c => c.Id("linearLayout1"));
            Assert.That(result.Length == count, "There are {0} rows when there there should be {1}.", result.Length, count);
        }

        protected string EnterTaskAndReturnToHomeScreen()
        {
            var numberOfRows = App.Query(c => c.Id("linearLayout1")).Length;
            var taskName = "Learn F# " + numberOfRows;
            TaskDetails = HomeScreen.NavigateToTaskDetails(App);
            TaskDetails.EnterTask(App, taskName, string.Format("Functional fun at {0}.", DateTime.Now), false);

            var d = TaskDetails;
            TaskDetails.NavigateTo<HomeScreen>(() =>
            {
                App.Tap(d.SaveButton);
                return App;
            });

            WaitForHomeScreen();
            return taskName;
        }
    }
    
}
