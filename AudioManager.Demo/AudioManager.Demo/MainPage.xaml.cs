using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AudioManager.Demo
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            await Audio.Manager.PlayBackgroundMusic("bgMusic.mp3");

            EnableBackgroundMusic.IsToggled = Audio.Manager.MusicOn;
            BackGroundVolume.Value = Audio.Manager.BackgroundMusicVolume;

            EnableEffects.IsToggled = Audio.Manager.EffectsOn;
            EffectsVolume.Value = Audio.Manager.EffectsVolume;
        }

        private void EnableBackgroundMusic_OnToggled(object sender, ToggledEventArgs e)
        {
            Audio.Manager.MusicOn = e.Value;
        }

        private void BackGroundVolume_OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            Audio.Manager.BackgroundMusicVolume = (float) e.NewValue;
        }

        private void EnableEffects_OnToggled(object sender, ToggledEventArgs e)
        {
            Audio.Manager.EffectsOn = e.Value;
        }

        private void EffectsVolume_OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            Audio.Manager.EffectsVolume = (float)e.NewValue;
        }
    }
}
