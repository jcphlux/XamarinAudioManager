# XamarinAudioManager

[![standard-readme compliant](https://img.shields.io/badge/readme%20style-standard-brightgreen.svg?style=flat-square)](https://github.com/RichardLitt/standard-readme)

![banner](https://github.com/jcphlux/XamarinAudioManager/blob/master/Images/XamarinAudio_Small.png)

> Cross platform Audio Manager for Xamarin

Xamarin Audio Manager provides a simple way to play audio files in Xamarin projects.

The Supported Xamarin Platforms are:
- iOS
- Android
- UWP
- Windows 8.1
- Windows Phone 8.1 //Sorta needs work but how many people use win phone ;)

Here is a sample showing how you can use Xamarin Audio Manager to set background music and play an effect sound.

```C#
await Audio.Manager.PlayBackgroundMusic("bgMusic.mp3");

await Audio.Manager.PlaySound("Drop.mp3");
```

## Table of Contents

- [Install](#install)
- [Usage](#usage)
- [Contribute](#contribute)
- [License](#license)

## Install

Install the [XamarinAudioManager NuGet Package](https://www.nuget.org/packages/XamarinAudioManager).

If you reference the package from a Xamarin Portable project, you will also need to reference the package from each Xamarin platform specific project. This is because the Xamarin Portable version of Xamarin Audio Manager doesn't contain the actual implementation of the audio APIs (because it differs from platform to platform), so referencing the package from a platform specific project will ensure that Xamarin Audio Manager is included in the app and used at runtime.

The target platforms need to initalize the AudioManager or the dll will be removed on compile. For iOS and Adndroind ths is done by calling a static class 'Initializer.Initialize();'. For Windows platforms you need to add a ref to the base canvas so the volume and mute fucntions will work so we are going to impliment an interface 'IAudioManagerContainer' to pass in that ref and initalize dll.

#### iOS Install

Add **Initializer.Initialize();** to Main.cs. See exaple below.

```cs
static void Main(string[] args)
{
    // if you want to use a different Application Delegate class from "AppDelegate"
    // you can specify it here.
    UIApplication.Main(args, null, "AppDelegate");

    Initializer.Initialize();
}
```

#### Android Install

Add **Initializer.Initialize();** to MainActivity.cs. See exaple below.

```cs
protected override void OnCreate(Bundle bundle)
{
    TabLayoutResource = Resource.Layout.Tabbar;
    ToolbarResource = Resource.Layout.Toolbar;

    base.OnCreate(bundle);

    global::Xamarin.Forms.Forms.Init(this, bundle);
    LoadApplication(new App());

    Initializer.Initialize();
}

```

#### UWP & Windows 8.1 Install

On Windows platforms we need to impliment an interface 'IAudioManagerContainer' in the MainPage.xaml.cs. We will use this interface to pass a ref to the base canvas. See example below.

```cs
public sealed partial class MainPage : IAudioManagerContainer
{
    public MainPage()
    {
        this.InitializeComponent();

        LoadApplication(new XamarinAudioManagerTest.App());

        AudioManagerContainer = this.Content as Canvas;
    }

    public Canvas AudioManagerContainer { get; set; }
}
```

#### Windows Phone 8.1 Install

```cs
Initializer.Initialize();
```

## Usage

```cs
```

## Contribute

Feel free to help out! [Open an issue](https://github.com/jcphlux/XamarinAudioManager/issues/new) or submit PRs.

Xamarin Audio Manager follows the [Contributor Covenant](https://github.com/jcphlux/XamarinAudioManager/blob/master/CODEOFCONDUCT.md) Code of Conduct.

## License

[MIT](https://github.com/jcphlux/XamarinAudioManager/blob/master/LICENSE) © John Cutburth II
