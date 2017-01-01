using System;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AudioManager.Interfaces;
using AudioManager.UWP.Interfaces;
using AudioManager.WinRT;
using Xamarin.Forms;

[assembly: Dependency(typeof(WinRtAudioManager))]
namespace AudioManager.WinRT
{
    class WinRtAudioManager : IAudioManager
    {
        #region Private Variables

        private Canvas _container;

        private MediaElement _backgroundMusic;
        private MediaElement _soundEffect;
        private string _backgroundSong = "";
        private bool _musicOn = true;
        private bool _effectsOn = true;

        #endregion

        #region Computed Properties

        public float BackgroundMusicVolume
        {
            get { return _backgroundMusic==null ? 
                    MusicVolume :
                    (float)_backgroundMusic.Volume; }
            set
            {
                MusicVolume = value;

                if (_backgroundMusic != null)
                    _backgroundMusic.Volume = value;
            }
        }

        public bool MusicOn
        {
            get { return _musicOn; }
            set
            {
                _musicOn = value;

                if (!MusicOn)
                    SuspendBackgroundMusic();
                else
                    RestartBackgroundMusic();

            }
        }
        public bool EffectsOn
        {
            get { return _effectsOn; }
            set
            {
                _effectsOn = value;

                if (!EffectsOn)
                    _soundEffect?.Stop();
            }
        }

        public float MusicVolume { get; set; } = 0.5f;

        public float EffectsVolume { get; set; } = 1.0f;

        public string SoundPath { get; set; } = "Sounds";

        #endregion

        #region Constructors

        public WinRtAudioManager()
        {
            var audioManagerContainer = ((Windows.UI.Xaml.Controls.Frame)Window.Current.Content).Content as IAudioManagerContainer;
            if (audioManagerContainer != null)
                _container = audioManagerContainer.AudioManagerContainer;
            // Initialize
            ActivateAudioSession();
        }

        #endregion

        #region Public Methods

        public void ActivateAudioSession()
        {
            // Initialize Audio
            //var session = AVAudioSession.SharedInstance();
            //session.SetCategory(AVAudioSessionCategory.Ambient);
            //session.SetActive(true);
        }

        public void DeactivateAudioSession()
        {
            //var session = AVAudioSession.SharedInstance();
            //session.SetActive(false);
        }

        public void ReactivateAudioSession()
        {
            //var session = AVAudioSession.SharedInstance();
            //session.SetActive(true);
        }

        public async void PlayBackgroundMusic(string filename)
        {
            // Music enabled?
            if (!MusicOn) return;

            // Any existing background music?
            //Stop and dispose of any background music
            _backgroundMusic?.Stop();

            StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Sounds");
            StorageFile file = await folder.GetFileAsync(filename);
            var stream = await file.OpenAsync(FileAccessMode.Read);

            // Initialize background music
            _backgroundMusic = new MediaElement();
            _backgroundMusic.Volume = MusicVolume;
            _backgroundMusic.MediaEnded += delegate
            {
                _container.Children.Remove(_backgroundMusic);
                _backgroundMusic = null;
            };
            _backgroundMusic.IsLooping = true;
            _backgroundMusic.AutoPlay = true;
            _backgroundMusic.RealTimePlayback = true;
            _container.Children.Add(_backgroundMusic);
            _backgroundMusic.SetSource(stream, file.ContentType);

            _backgroundSong = filename;

        }

        public void StopBackgroundMusic()
        {
            // If any background music is playing, stop it
            _backgroundSong = "";
            _backgroundMusic?.Stop();
        }

        public void SuspendBackgroundMusic()
        {
            // If any background music is playing, stop it
            _backgroundMusic?.Pause();
        }

        public void RestartBackgroundMusic()
        {
            // Music enabled?
            if (!MusicOn) return;

            // Was a song previously playing?
            if (_backgroundSong == "") return;

            // Restart song to fix issue with wonky music after sleep
            PlayBackgroundMusic(_backgroundSong);
        }

        public async void PlaySound(string filename)
        {
            // Music enabled?
            if (!EffectsOn) return;

            // Any existing sound effect?
            //Stop and dispose of any sound effect
            _soundEffect?.Stop();

            // Initialize sound
            _soundEffect = new MediaElement();
            _soundEffect.Volume = MusicVolume;
            _soundEffect.MediaEnded += delegate
            {
                _container.Children.Remove(_soundEffect);
                _soundEffect = null;
            };
            _soundEffect.IsLooping = false;
            _soundEffect.AutoPlay = true;
            StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(SoundPath);
            StorageFile file = await folder.GetFileAsync(filename);
            var stream = await file.OpenAsync(FileAccessMode.Read);
            _soundEffect.SetSource(stream, file.ContentType);
        }

        #endregion
    }
}
