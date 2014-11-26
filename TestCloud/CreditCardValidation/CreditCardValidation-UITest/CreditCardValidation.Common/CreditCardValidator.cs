using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreditCardValidation.Common
{

    public interface ICreditCardValidator
    {
        bool IsCCValid(string creditCardNumber, out string errMessage);
    }

    public class CreditCardValidator : ICreditCardValidator
    {
        /// <summary>
        ///   Runs a simple check to see if the credit card number is valid or not.
        /// </summary>
        /// <returns><c>true</c> if this instance is CC valid the specified errMessage; otherwise, <c>false</c>.</returns>
        /// <param name="creditCardNumber">The credit card number to validated.</param>
        /// <param name="errMessage">Error message.</param>
        public bool IsCCValid(string creditCardNumber, out string errMessage)
        {
            var isValid = true;
            errMessage = "";

            if (string.IsNullOrWhiteSpace(creditCardNumber))
            {
                errMessage = "This is not a credit card number.";
                isValid = false;
            }
            else if (creditCardNumber.Length < 16)
            {
                errMessage = "Credit card number is too short.";
                isValid = false;
            } else if (creditCardNumber.Length > 16)
            {
                errMessage = "Credit card number is too long.";
                isValid = false;
            }

            return isValid;
        }
    }
}
