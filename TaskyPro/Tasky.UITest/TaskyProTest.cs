using NUnit.Framework;
using Xamarin.UITest;

namespace Tasky.UITest
{
    public abstract class TaskyProTest
    {
        public IApp App { get; protected set; }

        [TestFixtureSetUp]
        public virtual void TestFixtureSetup()
        {
            BootStrapper.Initialize();
        }

        [SetUp]
        public virtual void SetUp()
        {
            App = BootStrapper.AppFactory.CreateApp();
        }
    }
}
