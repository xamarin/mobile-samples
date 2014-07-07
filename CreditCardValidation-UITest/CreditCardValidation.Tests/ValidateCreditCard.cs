using System;
using NUnit.Framework;
using Xamarin.UITest.iOS;
using Xamarin.UITest;
using Xamarin.UITest.Queries;
using System.Linq;
using System.Reflection;
using System.IO;

namespace CreditCardValidation.Tests
{
    [TestFixture]
    public class ValidateCreditCard
    {
        static readonly Func<AppQuery, AppQuery> EditTextView = c => c.Marked("CreditCardTextField");
        static readonly Func<AppQuery, AppQuery> ValidateButton = c => c.Marked("ValidateButton");
        static readonly Func<AppQuery, AppQuery> ErrorMessageLabel = c => c.Marked("ErrorMessagesTextField");

        static readonly Func<AppQuery, AppQuery> SuccessScreenNavBar = c=>c.Class("UINavigationBar").Id("Valid Credit Card");
        static readonly Func<AppQuery, AppQuery> SuccessMessageLabel = c => c.Class("UILabel").Text("The credit card number is valid!");

        iOSApp _app;

        public string PathToIPA { get; set; }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            string currentFile = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
            FileInfo fi = new FileInfo(currentFile);
            string dir = fi.Directory.Parent.Parent.Parent.FullName;
            PathToIPA = Path.Combine(dir, "CreditCardValidation.iOS", "bin", "iPhoneSimulator", "Debug", "CreditCardValidationiOS.app");
        }


        [SetUp]
        public void SetUp()
        {

            _app = ConfigureApp
                    .iOS
                    .Debug()
                    .AppBundle(PathToIPA)
                    .StartApp();
        }

        [Test]
        public void CreditCardNumber_TooShort_DisplayErrorMessage()
        {
            /* Arrange - set up our queries for the views */
            // Nothing to do here - the queries are already defined.

            /* Act */
            _app.EnterText(EditTextView, new string('9', 15));
            _app.Tap(ValidateButton);

            /* Assert */
            AppResult[] result = _app.Query(c => c.Class("UILabel").Text("Credit card number is to short."));
            Assert.IsTrue(result.Any(), "The error message is not being displayed.");
        }

        [Test]
        public void CreditCardNumber_TooLong_DisplayErrorMessage()
        {
            /* Arrange - set up our queries for the views */
            // Nothing to do here - the queries are already defined.

            /* Act */
            _app.EnterText(EditTextView, new string('9', 17));
            _app.Tap(ValidateButton);

            /* Assert */
            AppResult[] result = _app.Query(c => c.Class("UILabel").Text("Credit card number is to long."));
            Assert.IsTrue(result.Any(), "The error message is not being displayed.");
        }

        [Test]
        public void CreditCardNumber_CorrectSize_DisplaySuccessScreen()
        {
            // Arrange - set up our queries for the views
            // Nothing to do here - the queries are already defined.

            /* Act */
            _app.EnterText(EditTextView, new string('9', 16));
            _app.Tap(ValidateButton);

            /* Assert */
            _app.WaitForElement(SuccessMessageLabel, "Valid Credit Card screen did not appear", TimeSpan.FromSeconds(5));
            AppResult[] results = _app.Query(SuccessMessageLabel);
            Assert.IsTrue(results.Any(), "The success message was not displayed on the screen");
        }
    }
}

