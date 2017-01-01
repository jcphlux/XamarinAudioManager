using Windows.UI.Xaml.Controls;

namespace AudioManager.Demo.UWP
{
    public sealed partial class MainPage : IAudioManagerContainer
    {
        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new AudioManager.Demo.App());

            AudioManagerContainer = this.Content as Canvas;
        }

        public Canvas AudioManagerContainer { get; set; }
    }
}
