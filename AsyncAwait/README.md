AsyncAwait
==========

This sample was first presented in the Xamarin Introduction to C# 5 Async webinar on August 15th, 2013. Re-live the excitement with these links to the video, slides and Xamarin docs.

[Webinar video](http://xamarin.wistia.com/medias/k27mc627xz)

[Miguel's slides](http://www.slideshare.net/Xamarin/xamarin-asyncwebinar-2013) [Craig's slides](http://www.slideshare.net/Xamarin/c-async-on-ios-and-android-craig-dunn-developer-evangelist-at-xamarin)

[Xamarin's Introduction to Async article](http://docs.xamarin.com/guides/cross-platform/advanced/async_support_overview)

The sample apps (for iOS and Android) has two examples:

* a  simple text & image download using the new C# 5 `async` and `await` syntax.
* another example that shows how to download multiple files in the background and report download progress using `IProgress<T>`/`Progress<T>`. It also demonstrates how to cancel the background downloads using a `CancellationTokenSource`.

Also check out [Miguel's blog post on the subject](http://tirania.org/blog/archive/2013/Aug-15.html).
