using System;
using NUnit.Framework;

namespace Tasky.UITest
{

    [TestFixture]
    public class TaskDetailsTests : TaskyProTest
    {
        HomeScreen _home;

        public override void SetUp()
        {
            base.SetUp();
            _home = new HomeScreen();
        }

        [Test]
        public void TaskDetails_DeleteExistingTask()
        {
            // Arrange  
            TaskDetailsScreen details;
            var taskName = EnterNewTask(out details);

            // Act
            _home.ViewTaskNamed(App, taskName);
            App.Tap(details.DeleteButton);
            WaitForHomeScreen();

            // Assert
            Assert.That(_home.HasOneTaskNamed(App, taskName), Is.False, "There is one or more tasks named " + taskName + ".");
        }

        [Test]
        public void TaskDetails_EnterNewTask_AppearInHomeScreen()
        {
            // Arrange

            // Act
            TaskDetailsScreen details;
            var taskName = EnterNewTask(out details);

            // Assert
            Assert.IsTrue(_home.HasOneTaskNamed(App, taskName), "There should be only one task with the name '" + taskName + "'");
        }

        [Test]
        public void TaskDetails_TouchCancelButton_BackToHomeScreen()
        {
            // Arrange
            var details = HomeScreen.NavigateToTaskDetails(App);
            details.EnterTask(App, "Learn F#", "Functional fun for the whole family!", false);

            // Act
            App.Tap(details.DeleteButton);

            // Assert
            WaitForHomeScreen();
        }

        string EnterNewTask(out TaskDetailsScreen details)
        {
            var numberOfRows = App.Query(c => c.Id("linearLayout1")).Length;
            var taskName = "Learn F# " + numberOfRows;
            details = HomeScreen.NavigateToTaskDetails(App);
            details.EnterTask(App, taskName, string.Format("Functional fun at {0}", DateTime.Now), false);

            var d = details;
            var home = details.NavigateTo<HomeScreen>(() =>
            {
                App.Tap(d.SaveButton);
                return App;
            });

            WaitForHomeScreen();
            return taskName;

        }

        void WaitForHomeScreen()
        {
            App.WaitForElement(_home.Marked);
            Assert.That(_home.IsDisplayed(App), Is.True, "Should have returned to the Home Screen");
        }
    }
    
}
