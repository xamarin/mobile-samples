# Xamarin cross-platform mobile samples
==============

This repository contains a number of samples that show how to share code between Xamarin.iOS and Xamarin.Android.

## License

See the [license file](LICENSE) and any additional license information attached to each sample.

## Samples submission guidelines

This repository welcomes contributions and suggestions. If you want to create a new sample, you need to work with an employee to help bring the new sample into the repository. Start by raising a [GitHub issue](https://github.com/xamarin/mobile-samples/issues/new) in this repository that outlines your proposed sample. Please note that samples in the MASTER branch of this repository shouldn't rely on preview or pre-release NuGet packages.

The Xamarin [sample gallery](https://docs.microsoft.com/samples/browse/?products=xamarin) is powered by this repository, and therefore each sample needs to comply with the following requirements:

- **Screenshots** - a folder called Screenshots that has at least one screen shot of the sample on each platform (preferably a screen shot for every page or every major piece of functionality). For an example of this, see [ios11/MapKitSample](https://github.com/xamarin/ios-samples/tree/master/ios11/MapKitSample/Screenshots).

- **Readme** - a `README.md` file that explains the sample, and contains metadata to help customers find it. For an example of this, see [ios11/MapKitSample](https://github.com/xamarin/ios-samples/tree/master/ios11/MapKitSample/README.md). The README file should begin with a YAML header (delimited by `---`) with the following keys/values:

  - **name** - must begin with `Xamarin -`

    - **description** - brief description of the sample (&lt; 150 chars) that appears in the sample code browser search

    - **page_type** - must be the string `sample`.

    - **languages** - coding language/s used in the sample, such as: `csharp`, `fsharp`, `vb`, `objc`

    - **products**: should be `xamarin` for every sample in this repo

    - **urlFragment**: although this can be auto-generated, please supply an all-lowercase value that represents the sample's path in this repo, except directory separators are replaced with dashes (`-`) and no other punctuation.

    Here is an example:

    ```yaml
    ---
    name: Xamarin - Cross-Platform Sample
    description: Brief 150 character or less description of the sample
    page_type: sample
    languages:
    - csharp
    products:
    - xamarin
    urlFragment: crossplatformsample
    ---
    # Heading 1

    rest of README goes here, including screenshot images and requirements/instructions to get it running
    ```

    > NOTE: This must be valid YAML, so some characters in the name or description will require the entire string to be surrounded by " or ' quotes.

- **Buildable solution and .csproj file** - the project _must_ build and have the appropriate project scaffolding (solution + .csproj files).

This approach ensures that all samples integrate with the Microsoft [sample code browser](https://docs.microsoft.com/samples/browse/?products=xamarin).

If you have any questions, don't hesitate to ask on the [Xamarin Forums](https://forums.xamarin.com/).

## GitHub Integration

We integrate tightly with Git to make sure we always provide working samples to our customers. This is achieved through a pre-commit hook that runs before your commit goes through, as well as a post-receive hook on GitHub's end that notifies our samples gallery server when changes go through.

To you, as a sample committer, this means that before you push to the repos, you should run the "install-hook.bat" or "install-hook.sh" (depending on whether you're on Windows or macOS/Linux, respectively). These will install the Git pre-commit hook. Now, whenever you try to make a Git commit, all samples in the repo will be validated. If any sample fails to validate, the commit is aborted; otherwise, your commit goes through and you can go ahead and push.

This strict approach is put in place to ensure that the samples we present to our customers are always in a good state, and to ensure that all samples integrate correctly with the sample gallery (README.md, Metadata.xml, etc). Note that the master branch of each sample repo is what we present to our customers for our stable releases, so they must *always* Just Work.

Should you wish to invoke validation of samples manually, simply run "validate.windows" or "validate.posix" (again, Windows vs macOS/Linux, respectively). These must be run from a Bash shell (i.e. a terminal on macOS/Linux or the Git Bash terminal on Windows).

If you have any questions, don't hesitate to ask!

## Other samples

### Galleries

We love samples! Application samples show off our platform and provide a great way for people to learn our stuff. And we even promote them as a first-class feature of the docs site. You can find our two sample galleries here:

- [Xamarin.Forms Samples](https://docs.microsoft.com/samples/browse/?term=Xamarin.Forms)

- [iOS Samples](https://docs.microsoft.com/samples/browse/?term=Xamarin.iOS)

- [Mac Samples](https://docs.microsoft.com/samples/browse/?term=Xamarin.Mac)

- [Android Samples](https://docs.microsoft.com/samples/browse/?term=Xamarin.Android)

### Sample GitHub Repositories

These sample galleries are populated by samples in our six sample GitHub repos:

- [https://github.com/xamarin/xamarin-forms-samples](https://github.com/xamarin/xamarin-forms-samples)

- [https://github.com/xamarin/mobile-samples](https://github.com/xamarin/mobile-samples)

- [https://github.com/xamarin/monotouch-samples](https://github.com/xamarin/ios-samples)

- [https://github.com/xamarin/mac-samples](https://github.com/xamarin/mac-samples)

- [https://github.com/xamarin/monodroid-samples](https://github.com/xamarin/monodroid-samples)

- [https://github.com/xamarin/mac-ios-samples](https://github.com/xamarin/mac-ios-samples)

The [mobile-samples](https://github.com/xamarin/mobile-samples) repository is for samples that are cross-platform.
The [mac-ios-samples](https://github.com/xamarin/mac-ios-samples) repository is for samples that are Mac/iOS only.
