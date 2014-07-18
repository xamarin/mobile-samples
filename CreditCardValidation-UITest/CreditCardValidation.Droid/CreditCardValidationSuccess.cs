
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace CreditCardValidation.Droid
{
    [Activity(Label = "Valid Credit Card", Theme = "@android:style/Theme.Holo.Light")]			
    public class CreditCardValidationSuccess : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.CreditCardValidationSuccess);
        }
    }
}

