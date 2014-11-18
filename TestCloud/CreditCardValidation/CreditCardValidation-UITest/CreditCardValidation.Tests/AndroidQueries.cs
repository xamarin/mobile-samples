using System;

using Xamarin.UITest.Queries;

namespace CreditCardValidation.Tests
{
    public class AndroidQueries : IScreenQueries
    {
        public Func<AppQuery, AppQuery> CreditCardNumberView { get { return c => c.Marked("creditCardNumberText"); } }

        public Func<AppQuery, AppQuery> ValidateButtonView { get { return c => c.Marked("validateButton"); } }

        public Func<AppQuery, AppQuery> ErrorMessageView { get { return c => c.Marked("errorMessagesText"); } }

        public Func<AppQuery, AppQuery> SuccessMessageView { get { return c => c.Marked("validationSuccessMessage").Text("The credit card number is valid!"); } }

        public Func<AppQuery, AppQuery> ShortCreditCardNumberView { get { return c => c.Marked("errorMessagesText").Text("Credit card number is to short."); } }

        public Func<AppQuery, AppQuery> LongCreditCardNumberView { get { return c => c.Marked("errorMessagesText").Text("Credit card number is to long."); } }

        public Func<AppQuery, AppQuery> MissingCreditCardNumberView { get { return c => c.Marked("errorMessagesText").Text("Credit card number is to short."); } }

        public Func<AppQuery, AppQuery> SuccessScreenNavBar { get { return c => c.Marked("action_bar_title").Text("Valid Credit Card"); } }
    }
}
