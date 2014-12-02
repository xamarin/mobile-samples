Xamarin.Auth helps you authenticate users via standard authentication mechanisms (e.g. OAuth 1.0 and 2.0), and store user credentials. It's also straightforward to add support for non-standard authentication schemes. The library is cross-platform, so once you learn it on iOS, you're all set on Android.

```csharp
using Xamarin.Auth;

var auth = new OAuth2Authenticator (
	clientId: "App ID from https://developers.facebook.com/apps",
	scope: "",
	authorizeUrl: new Uri ("https://m.facebook.com/dialog/oauth/"),
	redirectUrl: new Uri ("http://www.facebook.com/connect/login_success.html"));

auth.Completed += (sender, eventArgs) => {
	DismissViewController (true, null);
	if (eventArgs.IsAuthenticated) {
		// Use eventArgs.Account to do wonderful things
	}
}

PresentViewController (auth.GetUI (), true, null);
```

It's that easy to authenticate users!

*Some screenshots assembled with [PlaceIt](http://placeit.breezi.com/).*
