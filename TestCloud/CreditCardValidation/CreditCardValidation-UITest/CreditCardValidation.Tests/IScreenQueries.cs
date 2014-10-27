using System;

using Xamarin.UITest.Queries;

namespace CreditCardValidation.Tests
{
    /// <summary>
    /// This interface is implemented by classes that will provide platform specific UITest queries to the
    /// views on a screen.
    /// </summary>
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
}
