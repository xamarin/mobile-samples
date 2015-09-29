Xamarin.Mobile is an API for accessing common platform features, such as
reading the user's address book and using the camera, across iOS,
Android, and Windows Phone.

The goal of Xamarin.Mobile is to decrease the amount of
platform-specific code needed to perform common tasks in multiplatform
apps, making development simpler and faster.

Xamarin.Mobile is currently a preview release and is subject to API
changes.

**Note:** The Windows Phone 7.1 version of the library requires the
Microsoft.Bcl.Async NuGet package. You'll need to restore this package
to use the samples or download this package to any WP7 app using
Xamarin.Mobile.

## Examples

To access the address book (requires `READ_CONTACTS` permissions
on Android):

```csharp
using Xamarin.Contacts;
// ...

var book = new AddressBook ();
//         new AddressBook (this); on Android
if (!await book.RequestPermission()) {
	Console.WriteLine ("Permission denied by user or manifest");
	return;
}

foreach (Contact contact in book.OrderBy (c => c.LastName)) {
	Console.WriteLine ("{0} {1}", contact.FirstName, contact.LastName);
}
```

To get the user's location (requires `ACCESS_COARSE_LOCATION` and
`ACCESS_FINE_LOCATION` permissions on Android):

```csharp
using Xamarin.Geolocation;
// ...

var locator = new Geolocator { DesiredAccuracy = 50 };
//            new Geolocator (this) { ... }; on Android

Position position = await locator.GetPositionAsync (timeout: 10000);

Console.WriteLine ("Position Status: {0}", position.Timestamp);
Console.WriteLine ("Position Latitude: {0}", position.Latitude);
Console.WriteLine ("Position Longitude: {0}", position.Longitude);
```

To take a photo:

```csharp
using Xamarin.Media;
// ...

var picker = new MediaPicker ();
if (!picker.IsCameraAvailable)
	Console.WriteLine ("No camera!");
else {
	try {
		MediaFile file = await picker.TakePhotoAsync (new StoreCameraMediaOptions {
			Name = "test.jpg",
			Directory = "MediaPickerSample"
		});

		Console.WriteLine (file.Path);
	} catch (OperationCanceledException) {
		Console.WriteLine ("Canceled");
	}
}
```

On Android (requires `WRITE_EXTERNAL_STORAGE` permissions):

```csharp
using Xamarin.Media;
// ...

protected override void OnCreate (Bundle bundle)
{
	var picker = new MediaPicker (this);
	if (!picker.IsCameraAvailable)
		Console.WriteLine ("No camera!");
	else {
		var intent = picker.GetTakePhotoUI (new StoreCameraMediaOptions {
			Name = "test.jpg",
			Directory = "MediaPickerSample"
		});
		StartActivityForResult (intent, 1);
	}
}

protected override async void OnActivityResult (int requestCode, Result resultCode, Intent data)
{
	// User canceled
	if (resultCode == Result.Canceled)
		return;

	MediaFile file = await data.GetMediaFileExtraAsync (this);
	Console.WriteLine (file.Path);
}
```