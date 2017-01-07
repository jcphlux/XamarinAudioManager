using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AudioManager;
using AudioManager.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(WinAudioManager))]
namespace AudioManager
{
    class WinAudioManager : IAudioManager
    {
        #region Private Variables

        private readonly Canvas _container;
        private readonly List<MediaElement> _soundEffects = new List<MediaElement>();

        private MediaElement _backgroundMusic;
        private string _backgroundSong = "";

        private bool _musicOn = true;
        private bool _effectsOn = true;
        private float _backgroundMusicVolume = 0.5f;
        private float _effectsVolume = 1.0f;

        #endregion

        #region Computed Properties

        public float BackgroundMusicVolume
        {
            get
            {
                return _backgroundMusicVolume;
            }
            set
            {
                _backgroundMusicVolume = value;

                if (_backgroundMusic != null)
                    _backgroundMusic.Volume = _backgroundMusicVolume;
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

                if (!EffectsOn && _soundEffects.Any())
                    foreach (var s in _soundEffects) s.Stop();
            }
        }

        public float EffectsVolume
        {
            get { return _effectsVolume; }
            set
            {
                _effectsVolume = value;

                if (_soundEffects.Any())
                    foreach (var s in _soundEffects) s.Volume = _effectsVolume;
            }
        }

        public string SoundPath { get; set; } = "Sounds";

        #endregion

        #region Constructors

        public WinAudioManager()
        {
            var audioManagerContainer = ((Windows.UI.Xaml.Controls.Frame)Window.Current.Content).Content as IAudioManagerContainer;
            _container = audioManagerContainer != null ? audioManagerContainer.AudioManagerContainer : new Canvas();
            // Initialize
            ActivateAudioSession();
        }

        #endregion

        #region Public Methods

        public void ActivateAudioSession()
        {
            // Initialize Audio
            //todo
        }

        public void DeactivateAudioSession()
        {
            //todo
        }

        public void ReactivateAudioSession()
        {
            //todo
        }

        public async Task<bool> PlayBackgroundMusic(string filename)
        {
            // Music enabled?
            if (!MusicOn) return false;

            // Any existing background music?
            //Stop and dispose of any background music
            if (_backgroundMusic != null)
            {
                _backgroundMusic.Stop();
                _container.Children.Remove(_backgroundMusic);
            }

            _backgroundSong = filename;

            // Initialize background music
            _backgroundMusic = await NewSound(filename, BackgroundMusicVolume, true);
            _container.Children.Add(_backgroundMusic);

            return true;
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

        public async Task<bool> RestartBackgroundMusic()
        {
            // Music enabled?
            if (!MusicOn) return false;

            // Was a song previously playing?
            if (_backgroundSong == "") return false;

            // Restart song to fix issue with wonky music after sleep
            return await PlayBackgroundMusic(_backgroundSong);
        }

        public async Task<bool> PlaySound(string filename)
        {
            // Music enabled?
            if (!EffectsOn) return false;

            var effect = await NewSound(filename, EffectsVolume);
            _soundEffects.Add(effect);
            _container.Children.Add(effect);

            return true;
        }

        private async Task<MediaElement> NewSound(string filename, float defaultVolume, bool isLooping = false)
        {
            StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(SoundPath);
            StorageFile file = await folder.GetFileAsync(filename);
            var stream = await file.OpenAsync(FileAccessMode.Read);

            // Initialize sound
            var sound = new MediaElement
            {
                Volume = defaultVolume,
                IsLooping = isLooping,
                AutoPlay = true,
                Visibility = Visibility.Collapsed
            };

            sound.MediaEnded += SoundOnMediaEnded;

            sound.SetSource(stream, file.ContentType);

            return sound;
        }

        private void SoundOnMediaEnded(object sender, RoutedEventArgs routedEventArgs)
        {
            var se = sender as MediaElement;

            _container.Children.Remove(se);

            if (se != _backgroundMusic)
                _soundEffects.Remove(se);

        }

        #endregion
    }
}
