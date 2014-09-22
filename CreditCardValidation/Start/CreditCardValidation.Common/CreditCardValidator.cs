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

    public class CreditCardValidator: ICreditCardValidator
    {
        /// <summary>
        /// Runs a simple check to see if the credit card number is valid or not.
        /// </summary>
        /// <returns><c>true</c> if this instance is CC valid the specified errMessage; otherwise, <c>false</c>.</returns>
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
