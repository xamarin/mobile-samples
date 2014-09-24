using NUnit.Framework;
using Xamarin.UITest;

namespace Tasky.UITest
{

    public abstract class TaskyProTest
    {
        protected IApp App { get; private set; }

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
