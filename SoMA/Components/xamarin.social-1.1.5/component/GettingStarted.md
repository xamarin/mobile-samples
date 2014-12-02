## 1. Create and configure an authenticator

Let's authenticate a user to access Facebook:

```csharp
using Xamarin.Auth;
...
var auth = new OAuth2Authenticator (
	clientId: "App ID from https://developers.facebook.com/apps",
	scope: "",
	authorizeUrl: new Uri ("https://m.facebook.com/dialog/oauth/"),
	redirectUrl: new Uri ("http://www.facebook.com/connect/login_success.html"));
```

Facebook uses OAuth 2.0 authentication, so we create an `OAuth2Authenticator`. Authenticators are responsible for managing the user interface and communicating with authentication services.

Authenticators take a variety of parameters; in this case, the application's ID, its authorization scope, and Facebook's various service locations are required.




## 2. Authenticate the user

While authenticators manage their own UI, it's up to you to initially present the authenticator's UI on the screen. This lets you control how the authentication UI is displayed–modally, in navigation controllers, in popovers, etc.

Before we present the UI, we need to start listening to the `Completed` event which fires when the user successfully authenticates or cancels. You can find out if the authentication succeeded by testing the `IsAuthenticated` property of `eventArgs`:

```csharp
auth.Completed += (sender, eventArgs) => {
	// We presented the UI, so it's up to us to dimiss it on iOS.
	DismissViewController (true, null);

	if (eventArgs.IsAuthenticated) {
		// Use eventArgs.Account to do wonderful things
	} else {
		// The user cancelled
	}
};
```

All the information gathered from a successful authentication is available in `eventArgs.Account`.

Now we're ready to present the login UI from `ViewDidAppear` on iOS:

```csharp
PresentViewController (auth.GetUI (), true, null);
```

The `GetUI` method returns `UINavigationControllers` on iOS, and `Intents` on Android. On Android, we would write the following code to present the UI from `OnCreate`:

```csharp
StartActivity (auth.GetUI (this));
```



## 3. Making requests

Since Facebook is an OAuth2 service, we'll make requests with `OAuth2Request` providing the account we retrieved from the `Completed` event. Assuming we're authenticated, we'll grab the user's info to demonstrate:

```csharp
var request = new OAuth2Request ("GET", new Uri ("https://graph.facebook.com/me"), null, eventArgs.Account);
request.GetResponseAsync().ContinueWith (t => {
	if (t.IsFaulted)
		Console.WriteLine ("Error: " + t.Exception.InnerException.Message);
	else {
		string json = t.Result.GetResponseText();
		Console.WriteLine (json);
	}
});
```


## 4. Store the account

Xamarin.Auth securely stores `Account` objects so that you don't always have to reauthenticate the user. The `AccountStore` class is responsible for storing `Account` information, backed by the [Keychain](https://developer.apple.com/library/ios/#documentation/security/Reference/keychainservices/Reference/reference.html) on iOS and a [KeyStore](http://developer.android.com/reference/java/security/KeyStore.html) on Android:

```csharp
// On iOS:
AccountStore.Create ().Save (eventArgs.Account, "Facebook");

// On Android:
AccountStore.Create (this).Save (eventArgs.Account, "Facebook");
```

Saved Accounts are uniquely identified using a key composed of the account's `Username` property and a "Service ID". The "Service ID" is any string that is used when fetching accounts from the store.

If an `Account` was previously saved, calling `Save` again will overwrite it. This is convenient for services that expire the credentials stored in the account object.




## 5. Retrieve stored accounts

You can fetch all `Account` objects stored for a given service:

```csharp
// On iOS:
IEnumerable<Account> accounts = AccountStore.Create ().FindAccountsForService ("Facebook");

// On Android:
IEnumerable<Account> accounts = AccountStore.Create (this).FindAccountsForService ("Facebook");
```

It's that easy.




## 6. Make your own authenticator

Xamarin.Auth includes OAuth 1.0 and OAuth 2.0 authenticators, providing support for thousands of popular services. For services that use traditional username/password authentication, you can roll your own authenticator by deriving from `FormAuthenticator`.

If you want to authenticate against an ostensibly unsupported service, fear not – Xamarin.Auth is extensible! It's very easy to create your own authenticators – just derive from any of the existing authenticators and start overriding methods.


