using System;
using NUnit.Framework;
using System.Dynamic;

namespace Tasky.UITest
{

    [TestFixture]
    public class TaskDetails_EditTask: TaskDetailsTest
    {
        string _oldName;
        string _taskName;
        string _notes;

        public override void TestFixtureSetup()
        {
            base.TestFixtureSetup();
            App = BootStrapper.AppFactory.CreateApp();

            _oldName = EnterTaskAndReturnToHomeScreen();

            _taskName = "Learn Functional Programming";
            _notes = "New Notes " + DateTime.Now.ToString();

            Home.UpdateTaskNamed(App, _oldName, _taskName, _notes);
        }


        public override void SetUp()
        {
            // We don't want to do anything here.
        }
        [Test]
        public void HomeScreen_shows_new_task_name()
        {
            AssertHomeScreenShowsTasks(1);


            Assert.IsTrue(Home.HasOneTaskNamed(App, _taskName));


        }

        [Test]
        public void HomeScreen_shows_new_task_note()
        {
            var result = App.Query(c => c.Id("linearLayout1").Descendant(null).Id("lblDescription").Text(_notes));
            App.Repl();
            Assert.That(result.Length == 1, "There should be one task with the updated note, not {0}.", result.Length); 

        }

    }
    
}
