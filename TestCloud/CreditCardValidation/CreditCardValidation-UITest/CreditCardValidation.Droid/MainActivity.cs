using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

using CreditCardValidation.Common;

namespace CreditCardValidation.Droid
{
    [Activity(Label = "@string/app_name", MainLauncher = true, Icon = "@drawable/icon", Theme = "@android:style/Theme.Holo.Light")]
    public class MainActivity : Activity
    {
        static readonly ICreditCardValidator _creditCardValidator = new CreditCardValidator();
        EditText _creditCardTextField;
        TextView _errorMessagesTextField;
        Button _validateButton;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            _errorMessagesTextField = FindViewById<TextView>(Resource.Id.errorMessagesText);
            _creditCardTextField = FindViewById<EditText>(Resource.Id.creditCardNumberText);
            _validateButton = FindViewById<Button>(Resource.Id.validateButton);
            _validateButton.Click += (sender, e) =>{
                                         _errorMessagesTextField.Text = String.Empty;
                                         string errMessage;
                                         bool isValid = _creditCardValidator.IsCCValid(_creditCardTextField.Text, out errMessage);

                                         if (isValid)
                                         {
                                             Intent i = new Intent(this, typeof(CreditCardValidationSuccess));
                                             StartActivity(i);
                                         }
                                         else
                                         {
                                             RunOnUiThread(() => { _errorMessagesTextField.Text = errMessage; });
                                         }
                                     };
        }
    }
}
