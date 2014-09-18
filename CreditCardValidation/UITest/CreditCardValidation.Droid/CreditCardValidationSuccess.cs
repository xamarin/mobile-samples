using Android.App;
using Android.OS;

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
