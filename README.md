Mobile Samples
==============

This repository contains a number of samples that show how to share code between Xamarin.iOS and Xamarin.Android.

License
-------

See the [license file](LICENSE) and any additional license information attached to each sample.

Samples Submission Guidelines
=============================

This repository welcomes contributions and suggestions. If you want to create a new sample, you need to work with an employee to help bring the new sample into the repository. Start by raising a [GitHub issue](https://github.com/xamarin/mobile-samples/issues/new) in this repository that outlines your proposed sample. Please note that samples in the MASTER branch of this repository shouldn't rely on preview or pre-release NuGet packages.

The Xamarin [sample gallery](https://developer.xamarin.com/samples/) is powered by this repository, and therefore each sample needs to comply with the following requirements:

* **Screenshots** - a folder called Screenshots that has at least one screen shot of the sample on each platform (preferably a screen shot for every page or every major piece of functionality). For an example of this, see [TaskyPro](https://github.com/xamarin/mobile-samples/tree/master/TaskyPro/Screenshots).

* **Readme** - a `README.md` file that has the name of the sample, a description, and author attribution. For an example of this, see [TaskyPro](https://github.com/xamarin/mobile-samples/blob/master/TaskyPro/README.md).

* **Metadata** - a `Metadata.xml` file that has the following information:

    * **ID** - a GUID for the sample.

    * **IsFullApplication** - a boolean value that indicates whether the sample is a full app, which could be submitted to an app store, or a feature sample.

    * **Brief** - a short description of what the sample does.

    * **Level** - the intended audience level for the sample: Beginner, Intermediate, or Advanced. Only the getting started samples are Beginner, as they are intended for people who are _just_ starting with the platform. Most samples are Intermediate, and a few, that dive deep into difficult APIs, should be Advanced.

    * **Minimum License Requirement** - Starter, Indie, Business, or Enterprise: denotes the license that a user has to have in order to build and run the sample.

    * **Tags**: a list of relevant tags for the app. See other metadata files for examples.

    * **SupportedPlatforms**: a comma-separated list of the supported platforms. Valid values are currently iOS, Android, and Windows.

    * **Gallery**: a boolean value that indicates whether the sample should appear in the Xamarin.Forms [sample gallery](https://developer.xamarin.com/samples/xamarin-forms/).

    For an example of a `Metadata.xml` file, see [TaskyPro](https://github.com/xamarin/mobile-samples/blob/master/TaskyPro/Metadata.xml).

* **Buildable solution and .csproj file** - the project _must_ build and have the appropriate project scaffolding (solution + .csproj).

This approach ensures that all samples integrate with the Xamarin [sample gallery](https://developer.xamarin.com/samples/).

If you have any questions, don't hesitate to ask on the [Xamarin Forums](https://forums.xamarin.com/).

## GitHub Integration

We integrate tightly with Git to make sure we always provide working samples to our customers. This is achieved through a pre-commit hook that runs before your commit goes through, as well as a post-receive hook on GitHub's end that notifies our samples gallery server when changes go through.

To you, as a sample committer, this means that before you push to the repos, you should run the "install-hook.bat" or "install-hook.sh" (depending on whether you're on Windows or OS X/Linux, respectively). These will install the Git pre-commit hook. Now, whenever you try to make a Git commit, all samples in the repo will be validated. If any sample fails to validate, the commit is aborted; otherwise, your commit goes through and you can go ahead and push.

This strict approach is put in place to ensure that the samples we present to our customers are always in a good state, and to ensure that all samples integrate correctly with the sample gallery (README.md, Metadata.xml, etc). Note that the master branch of each sample repo is what we present to our customers for our stable releases, so they must *always* Just Work.

Should you wish to invoke validation of samples manually, simply run "validate.windows" or "validate.posix" (again, Windows vs OS X/Linux, respectively). These must be run from a Bash shell (i.e. a terminal on OS X/Linux or the Git Bash terminal on Windows).

If you have any questions, don't hesitate to ask!



# Other samples

## Galleries

We love samples! Application samples show off our platform and provide a great way for people to learn our stuff. And we even promote them as a first-class feature of the docs site. You can find our two sample galleries here:

* [Xamarin.Forms Samples](http://developer.xamarin.com/samples/xamarin-forms/all/)

* [iOS Samples](http://developer.xamarin.com/samples/ios/all/)

* [Mac Samples](http://developer.xamarin.com/samples/mac/all/)

* [Android Samples](http://developer.xamarin.com/samples/android/all/)

## Sample GitHub Repositories

These sample galleries are populated by samples in our six sample GitHub repos:

* [https://github.com/xamarin/xamarin-forms-samples](https://github.com/xamarin/xamarin-forms-samples)

* [https://github.com/xamarin/mobile-samples](https://github.com/xamarin/mobile-samples)

* [https://github.com/xamarin/monotouch-samples](https://github.com/xamarin/monotouch-samples)

* [https://github.com/xamarin/mac-samples](https://github.com/xamarin/mac-samples)

* [https://github.com/xamarin/monodroid-samples](https://github.com/xamarin/monodroid-samples)

* [https://github.com/xamarin/mac-ios-samples](https://github.com/xamarin/mac-ios-samples)

The [mobile-samples](https://github.com/xamarin/mobile-samples) repository is for samples that are cross-platform.
The [mac-ios-samples](https://github.com/xamarin/mac-ios-samples) repository is for samples that are Mac/iOS only.