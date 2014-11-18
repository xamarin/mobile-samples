using System;

using Xamarin.UITest.Queries;

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
        Func<AppQuery, AppQuery> MissingCreditCardNumberView { get; }
    }
}
