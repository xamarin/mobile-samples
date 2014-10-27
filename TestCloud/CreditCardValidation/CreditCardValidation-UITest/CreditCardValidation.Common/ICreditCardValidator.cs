namespace CreditCardValidation.Common
{
    /// <summary>
    /// This interface is implemented by classes that will validate a credit card number.
    /// </summary>
    public interface ICreditCardValidator
    {
        /// <summary>
        ///   Runs a simple check to see if the credit card number is valid or not.
        /// </summary>
        /// <returns><c>true</c> if this instance is CC valid the specified errMessage; otherwise, <c>false</c>.</returns>
        /// <param name="creditCardNumber">The credit card number to validate.</param>
        /// <param name="errMessage">If there is an error, this will contain the error message.</param>
        bool IsCCValid(string creditCardNumber, out string errMessage);
    }
}