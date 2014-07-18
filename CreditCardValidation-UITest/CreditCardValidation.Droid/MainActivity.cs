using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace CreditCardValidation.Droid
{
    [Activity(Label = "@string/app_name", MainLauncher = true, Icon = "@drawable/icon", Theme = "@android:style/Theme.Holo.Light")]
    public class MainActivity : Activity
    {
        Button _validateButton;
        EditText _creditCardTextField;
        TextView _errorMessagesTextField;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            _errorMessagesTextField = FindViewById<TextView>(Resource.Id.errorMessagesText);
            _creditCardTextField = FindViewById<EditText>(Resource.Id.creditCardNumberText);
            _validateButton = FindViewById<Button>(Resource.Id.validateButton);
            _validateButton.Click += (sender, e) => 
            {
                _errorMessagesTextField.Text = String.Empty;
                string errMessage;

                if (IsCCValid(out errMessage))
                {
                    var i = new Intent(this, typeof(CreditCardValidationSuccess));
                    StartActivity(i);
                }
                else
                {
                    RunOnUiThread( () => {
                        _errorMessagesTextField.Text = errMessage;

                    });
                }
            };

        }

         bool IsCCValid(out string errMessage)
        {
            errMessage = "";

            if (_creditCardTextField.Text.Length < 16)
            {
                errMessage = "Credit card number is to short.";
                return false;
            }
            else if (_creditCardTextField.Text.Length > 16)
            {
                errMessage = "Credit card number is to long.";
                return false;
            }

            return true;

        }
    }
}


