Xamarin.Mobile is an API for accessing common platform features, such as
reading the user's address book and using the camera, across iOS,
Android, and Windows Phone.

## Examples

To access the address book (requires `READ_CONTACTS` permissions
on Android):

```csharp
using Xamarin.Contacts;
...

var book = new AddressBook ();
//         new AddressBook (this); on Android
book.RequestPermission().ContinueWith (t => {
	if (!t.Result) {
		Console.WriteLine ("Permission denied by user or manifest");
		return;
	}

	foreach (Contact contact in book.OrderBy (c => c.LastName)) {
		Console.WriteLine ("{0} {1}", contact.FirstName, contact.LastName);
	}
});
```

To get the user's location (requires `ACCESS_COARSE_LOCATION` and
`ACCESS_FINE_LOCATION` permissions on Android):

```csharp
using Xamarin.Geolocation;
...

var locator = new Geolocator { DesiredAccuracy = 50 };
//            new Geolocator (this) { ... }; on Android
locator.GetPositionAsync (timeout: 10000).ContinueWith (t => {
	Console.WriteLine ("Position Status: {0}", t.Result.Timestamp);
	Console.WriteLine ("Position Latitude: {0}", t.Result.Latitude);
	Console.WriteLine ("Position Longitude: {0}", t.Result.Longitude);
});
```

To take a photo (requires `WRITE_EXTERNAL_STORAGE` permissions on
Android):

```csharp
using Xamarin.Media;
...

var picker = new MediaPicker ();
//           new MediaPicker (this); on Android
if (!picker.IsCameraAvailable)
	Console.WriteLine ("No camera!");
else {
	picker.TakePhotoAsync (new StoreCameraMediaOptions {
		Name = "test.jpg",
		Directory = "MediaPickerSample"
	}).ContinueWith (t => {
		if (t.IsCanceled) {
			Console.WriteLine ("User canceled");
			return;
		}
		Console.WriteLine (t.Result.Path);
	});
}
```