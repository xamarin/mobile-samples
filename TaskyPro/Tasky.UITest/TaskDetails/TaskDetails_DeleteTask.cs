using System;
using NUnit.Framework;
using System.Dynamic;

namespace Tasky.UITest
{

    [TestFixture]
    public class TaskDetails_DeleteTask : TaskDetailsTest
    {
        string _taskName;

        public override void SetUp()
        {
            base.SetUp();
            _taskName = EnterTaskAndReturnToHomeScreen();
        }

        [Test]
        public void TaskDetails_DeletedTask_ShouldNotAppearInHomeScreen()
        { 
            Home.ViewTaskNamed(App, _taskName);
            App.Tap(TaskDetails.DeleteButton);

            WaitForHomeScreen();
            Assert.That(Home.HasOneTaskNamed(App, _taskName), Is.False, "The task shold not appear in the home screen.");
        }


        [Test]
        public void TaskDetails_DeleteExistingTask()
        {
            // Arrange  
            var taskName = EnterTaskAndReturnToHomeScreen();

            // Act
            Home.ViewTaskNamed(App, taskName);
            App.Tap(TaskDetails.DeleteButton);
            WaitForHomeScreen();

            // Assert
            Assert.That(Home.HasOneTaskNamed(App, taskName), Is.False, "There is one or more tasks named " + taskName + ".");
        }
    }
}
