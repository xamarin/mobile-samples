## Sharing

We'll demonstrate sharing using Facebook. In order to share with Facebook, you'll need to have created
a Facebook app at https://developers.facebook.com/apps. Use the "Website with Facebook Login" integration
and ensure you've added `publish_stream` in the "Extended Permissions" section.

To share an item, first create the service, create the item and then present the share UI:

```csharp
using Xamarin.Social;
using Xamarin.Social.Services;
...

public override void ViewDidAppear (bool animated)
{
	base.ViewDidAppear (animated);

	// 1. Create the service
	var facebook = new FacebookService {
		ClientId = "<App ID from developers.facebook.com/apps>",
		RedirectUrl = new System.Uri ("<Redirect URL from developers.facebook.com/apps>")
	};

	// 2. Create an item to share
	var item = new Item { Text = "Xamarin.Social is the bomb.com." };
	item.Links.Add (new Uri ("http://github.com/xamarin/xamarin.social"));

	// 3. Present the UI on iOS
	var shareController = facebook.GetShareUI (item, result => {
		// result lets you know if the user shared the item or canceled
		DismissViewController (true, null);
	});
	PresentViewController (shareController, true, null);
}
```

If you're on Android, then you should present the share UI using an intent:

```csharp
protected override void OnCreate (Bundle bundle)
{
	base.OnCreate (bundle);

	// 1. Create the service
	var facebook = new FacebookService { ClientId = "<App ID from developers.facebook.com/apps>" };

	// 2. Create an item to share
	var item = new Item { Text = "Xamarin.Social is the bomb.com." };
	item.Links.Add (new Uri ("http://github.com/xamarin/xamarin.social"));

	// 3. Present the UI on Android
	var shareIntent = facebook.GetShareUI (this, item, result => {
		// result lets you know if the user shared the item or canceled
	});
	StartActivityForResult (shareIntent, 42);
}
```

## Services

Xamarin.Social comes with a variety of services that you can use to
share items, and can be created with the following credentials:

* [App.net](https://alpha.app.net/developer/apps/) `new AppDotNetService { ClientId }`
* [Facebook](http://developers.facebook.com) `new FacebookService { ClientId }`
* [Flickr](http://www.flickr.com/services/api/) `new FlickrService { ConsumerKey, ConsumerSecret }`
* [Twitter](http://dev.twitter.com) `new TwitterService { ConsumerKey, ConsumerSecret }`
* [Twitter](http://dev.twitter.com)* `new Twitter5Service ()`

\* `Twitter5Service` uses iOS 5-specific UI and account settings.

## Share Items

To share some text, links, or images, create an `Item` object and call
`GetShareUI`. The share UI allows the user to select the account that
they want to use, and allows the user to edit the item's text before it
is posted.

Items have properties for Text, Images, Files, and Links; however, not
all services support sharing all of these types of media. Use these
`Service` properties to query the limitations of different services:

* `MaxTextLength`
* `MaxLinks`
* `MaxImages`
* `MaxFiles`

As an alternative to presenting the share UI, you can share items
directly using the `ShareItemAsync` method of the service.

## Social APIs

If you want to do more than basic sharing, you can access arbitrary
service APIs using `CreateRequest`:

```csharp
var request = facebook.CreateRequest ("GET", new Uri ("https://graph.facebook.com/me/feed"), account);
request.GetResponseAsync ().ContinueWith (response => {
	// parse the JSON in response.GetResponseText ()
});
```

The service will automatically authenticate the request for you.

## Authentication

Xamarin.Social uses the Xamarin.Auth library to fetch and store `Account` objects. 

Each service exposes a `GetAuthenticateUI` method that returns a
`Xamarin.Auth.Authenticator` object that you can use to authenticate the
user. Doing so will automatically store the authenticated account so
that it can be used later.

You can retrieve stored accounts with `GetAccountsAsync`:

```csharp
facebook.GetAccountsAsync ().ContinueWith (accounts => {
	// accounts is an IEnumerable<Account> of saved accounts
});
```
