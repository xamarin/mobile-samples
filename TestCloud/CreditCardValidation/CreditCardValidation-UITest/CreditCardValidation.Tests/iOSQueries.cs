using System;

using Xamarin.UITest.Queries;

namespace CreditCardValidation.Tests
{
    public class iOSQueries : IScreenQueries
    {
        public Func<AppQuery, AppQuery> CreditCardNumberView { get { return c => c.Marked("CreditCardTextField"); } }

        public Func<AppQuery, AppQuery> ValidateButtonView { get { return c => c.Marked("ValidateButton"); } }

        public Func<AppQuery, AppQuery> ErrorMessageView { get { return c => c.Marked("ErrorMessagesTextField"); } }

        public Func<AppQuery, AppQuery> SuccessMessageView { get { return c => c.Marked("CreditCardIsValidLabel").Text("The credit card number is valid!"); } }

        public Func<AppQuery, AppQuery> ShortCreditCardNumberView { get { return c => c.Marked("ErrorMessagesTextField").Text("Credit card number is to short."); } }

        public Func<AppQuery, AppQuery> LongCreditCardNumberView { get { return c => c.Marked("ErrorMessagesTextField").Text("Credit card number is to long."); } }

        public Func<AppQuery, AppQuery> MissingCreditCardNumberView { get { return c => c.Marked("ErrorMessagesTextField").Text("Credit card number is to short."); } }

        public Func<AppQuery, AppQuery> SuccessScreenNavBar { get { return c => c.Class("UINavigationBar").Id("Valid Credit Card"); } }
    }
}
