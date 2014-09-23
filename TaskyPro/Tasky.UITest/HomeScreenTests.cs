using NUnit.Framework;
using Ninject;
using Xamarin.UITest;

namespace Tasky.UITest
{
    [TestFixture]
    public class HomeScreenTests
    {
        IApp _app;
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            BootStrapper.Initialize();
        }

        [SetUp]
        public void SetUp()
        {
            _app = BootStrapper.Container.Get<IApp>();
        }

        [Test]
        public void HomeScreen_ShouldDisplay_AddTaskButton()
        {
            Assert.IsNotNull(_app, "App not set");
        }

    }
}

