using System;
using NUnit.Framework;
using System.Dynamic;

namespace Tasky.UITest
{
    [TestFixture]
    public class TaskDetails_AddTask : TaskDetailsTest
    {
        [Test]
        public void TaskDetails_EnterNewTask_AppearInHomeScreen()
        {
            // Arrange

            // Act
            var taskName = EnterTaskAndReturnToHomeScreen();

            // Assert
            Assert.IsTrue(Home.HasOneTaskNamed(App, taskName), "There should be only one task with the name '" + taskName + "'");
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
            AssertHomeScreenShowsTasks(0);
        }
    }
}
