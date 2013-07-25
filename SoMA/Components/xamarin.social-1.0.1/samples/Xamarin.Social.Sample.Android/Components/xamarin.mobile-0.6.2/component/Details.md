Xamarin.Mobile is an API for accessing common platform features, such as
reading the user's address book and using the camera, across iOS,
Android, and Windows Phone.

The goal of Xamarin.Mobile is to decrease the amount of
platform-specific code needed to perform common tasks in multiplatform
apps, making development simpler and faster.

## Examples

To access the address book:

```csharp
var book = new AddressBook ();
book.RequestPermission().ContinueWith (t => {
	if (!t.Result) {
		Console.WriteLine ("Permission denied by user or manifest");
		return;
	}

	foreach (Contact contact in book.OrderBy (c => c.LastName)) {
		Console.WriteLine ("{0} {1}", contact.FirstName, contact.LastName);
	}
}
```

To get the user's location:

```csharp
var locator = new Geolocator { DesiredAccuracy = 50 };
locator.GetPositionAsync (timeout: 10000).ContinueWith (t => {
	Console.WriteLine ("Position Status: {0}", t.Result.Timestamp);
	Console.WriteLine ("Position Latitude: {0}", t.Result.Latitude);
	Console.WriteLine ("Position Longitude: {0}", t.Result.Longitude);
});
```

To take a photo:

```csharp
var picker = new MediaPicker ();
if (!picker.IsCameraAvailable)
	Console.WriteLine ("No camera!")
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