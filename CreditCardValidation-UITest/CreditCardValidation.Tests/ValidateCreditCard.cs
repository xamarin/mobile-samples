using System;
using NUnit.Framework;
using Xamarin.UITest.iOS;
using Xamarin.UITest;
using Xamarin.UITest.Queries;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Security.Cryptography;

namespace CreditCardValidation.Tests
{
    public interface IScreenQueries
    {
        Func<AppQuery, AppQuery> CreditCardNumberView { get; }
        Func<AppQuery, AppQuery> ValidateButtonView { get; }
        Func<AppQuery, AppQuery> ErrorMessageView { get; }
        Func<AppQuery, AppQuery> SuccessMessageView { get; }
        Func<AppQuery, AppQuery> ShortCreditCardNumberView { get; }
        Func<AppQuery, AppQuery> LongCreditCardNumberView { get; }
        Func<AppQuery, AppQuery> SuccessScreenNavBar { get; }
    }

    public class AndroidQueries : IScreenQueries 
    {
        public Func<AppQuery, AppQuery> CreditCardNumberView
        {
            get
            {
                return c => c.Id("creditCardNumberText");
            }
        }
        public Func<AppQuery, AppQuery> ValidateButtonView
        {
            get
            {
                return c => c.Id("validateButton");
            }
        }
        public Func<AppQuery, AppQuery> ErrorMessageView
        {
            get
            {
                return c => c.Id("errorMessagesText");
            }
        }
        public Func<AppQuery, AppQuery> SuccessMessageView
        {
            get
            {
                return c => c.Id("validationSuccessMessage").Text("The credit card number is valid!");
            }
        }
        public Func<AppQuery, AppQuery> ShortCreditCardNumberView
        {
            get
            {
                return c => c.Id("errorMessagesText").Text("Credit card number is to short.");
            }
        }
        public Func<AppQuery, AppQuery> LongCreditCardNumberView
        {
            get
            {
                return c => c.Id("errorMessagesText").Text("Credit card number is to long.");
            }
        }
        public Func<AppQuery, AppQuery> SuccessScreenNavBar
        {
            get
            {
                return c => c.Id("action_bar_title").Text("Valid Credit Card");
            }
        }
    }

    public class iOSQueries : IScreenQueries
    {
        public Func<AppQuery, AppQuery> CreditCardNumberView
        {
            get { return c => c.Marked("CreditCardTextField"); }
        }
        public Func<AppQuery, AppQuery> ValidateButtonView
        {
            get { return c => c.Marked("ValidateButton"); }
        }
        public Func<AppQuery, AppQuery> ErrorMessageView
        {
            get { return c => c.Marked("ErrorMessagesTextField"); }
        }
        public Func<AppQuery, AppQuery> SuccessMessageView
        {
            get { return c => c.Class("UILabel").Text("The credit card number is valid!"); }
        }
        public Func<AppQuery, AppQuery> ShortCreditCardNumberView 
        {
            get {return c => c.Class("UILabel").Text("Credit card number is to short."); }
        }
        public Func<AppQuery, AppQuery> LongCreditCardNumberView
        {
            get { return c => c.Class("UILabel").Text("Credit card number is to long."); }
        }
        public Func<AppQuery, AppQuery> SuccessScreenNavBar
        {
            get { return c=>c.Class("UINavigationBar").Id("Valid Credit Card");            }
        }
    }

    [TestFixture]
    public class ValidateCreditCard
    {
        IScreenQueries _queries;
        IApp _app;

        public string PathToIPA { get; private set; }
        public string PathToAPK { get; private set; }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            if (TestEnvironment.IsTestCloud)
            {
                PathToAPK = String.Empty;
                PathToIPA = String.Empty;
            }
            else
            {
                string currentFile = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
                FileInfo fi = new FileInfo(currentFile);
                string dir = fi.Directory.Parent.Parent.Parent.FullName;

                PathToIPA = Path.Combine(dir, "CreditCardValidation.iOS", "bin", "iPhoneSimulator", "Debug", "CreditCardValidationiOS.app");
                PathToAPK = Path.Combine(dir, "CreditCardValidation.Droid", "bin", "Release", "CreditCardValidation.Droid.APK");
            }
        }


        [SetUp]
        public void SetUp()
        {
            if (TestEnvironment.IsTestCloud)
            {
                if (TestEnvironment.Platform.Equals(TestPlatform.TestCloudiOS))
                {
                    _app = ConfigureApp
                        .iOS
                        .StartApp();
                    _queries = new iOSQueries();
                }
                else
                {
                    _queries = new AndroidQueries();
                    _app = ConfigureApp
                        .Android
                        .StartApp();
                }
            }
            else
            {
//                _app = ConfigureApp
//                    .iOS
//                    .AppBundle(PathToIPA)
//                    .StartApp();
//                _queries = new iOSQueries();

                _app = ConfigureApp
                    .Android
                    .ApkFile(PathToAPK)
                    .StartApp();
                _queries = new AndroidQueries();
            }
        }

        [Test]
        public void CreditCardNumber_TooShort_DisplayErrorMessage()
        {
            /* Arrange - set up our queries for the views */
            // Nothing to do here - the queries are already defined.

            /* Act */
            _app.EnterText(_queries.CreditCardNumberView, new string('9', 15));
            _app.Tap(_queries.ValidateButtonView);

            /* Assert */
            AppResult[] result = _app.Query(_queries.ShortCreditCardNumberView);
            Assert.IsTrue(result.Any(), "The error message is not being displayed.");
        }

        [Test]
        public void CreditCardNumber_TooLong_DisplayErrorMessage()
        {
            /* Arrange - set up our queries for the views */
            // Nothing to do here - the queries are already defined.

            /* Act */
            _app.EnterText(_queries.CreditCardNumberView, new string('9', 17));
            _app.Tap(_queries.ValidateButtonView);

            /* Assert */
            AppResult[] result = _app.Query(_queries.LongCreditCardNumberView);
            Assert.IsTrue(result.Any(), "The error message is not being displayed.");
        }

        [Test]
        public void CreditCardNumber_CorrectSize_DisplaySuccessScreen()
        {

            // Arrange - set up our queries for the views
            // Nothing to do here - the queries are already defined.

            /* Act */
            _app.EnterText(_queries.CreditCardNumberView, new string('9', 16));
            _app.Tap(_queries.ValidateButtonView);

            /* Assert */
            _app.WaitForElement(_queries.SuccessScreenNavBar, "Valid Credit Card screen did not appear", TimeSpan.FromSeconds(5));
            AppResult[] results = _app.Query(_queries.SuccessMessageView);
            Assert.IsTrue(results.Any(), "The success message was not displayed on the screen");
        }
    }
}

