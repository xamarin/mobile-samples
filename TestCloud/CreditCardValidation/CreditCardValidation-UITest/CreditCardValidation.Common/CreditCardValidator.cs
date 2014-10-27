namespace CreditCardValidation.Common
{
    public class CreditCardValidator : ICreditCardValidator
    {
        /// <summary>
        ///   Runs a simple check to see if the credit card number is valid or not.
        /// </summary>
        /// <returns><c>true</c> if this instance is CC valid the specified errMessage; otherwise, <c>false</c>.</returns>
        /// <param name="creditCardNumber">The credit card number to validate.</param>
        /// <param name="errMessage">If there is an error, this will contain the error message.</param>
        public bool IsCCValid(string creditCardNumber, out string errMessage)
        {
            bool isValid = true;
            errMessage = "";

            if (string.IsNullOrWhiteSpace(creditCardNumber))
            {
                errMessage = "Please enter a credit card number.";
                isValid = false;
            }

            if (creditCardNumber.Length < 16)
            {
                errMessage = "Credit card number is to short.";
                isValid = false;
            }
            if (creditCardNumber.Length > 16)
            {
                errMessage = "Credit card number is to long.";
                isValid = false;
            }

            return isValid;
        }
    }
}
