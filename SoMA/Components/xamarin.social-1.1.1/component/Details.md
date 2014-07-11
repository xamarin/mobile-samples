Xamarin.Social posts statuses, links, images, and other media to social networks using
a simple, cross-platform API. With Xamarin.Social, you can easily:

 1. Share text and images on social networks.
 2. Access social network APIs using authenticated requests.
 3. Automatically and securely store user credentials using Xamarin.Auth.

Xamarin.Social currently works with these social networks, and can be extended to support
custom services:

 * [App.net](http://alpha.app.net)
 * [Facebook](http://facebook.com)
 * [Flickr](http://www.flickr.com)
 * [Twitter](http://twitter.com)

An example for sharing a link with Facebook on iOS:

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

*Some screenshots assembled with [PlaceIt](http://placeit.breezi.com/).*
