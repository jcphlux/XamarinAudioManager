using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Android.Media;
using AudioManager.Droid;
using AudioManager.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(DroidAudioManager))]
namespace AudioManager.Droid
{
    class DroidAudioManager : IAudioManager
    {
        #region Private Variables

        private readonly List<MediaPlayer> _soundEffects = new List<MediaPlayer>();

        private MediaPlayer _backgroundMusic;
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
                    _backgroundMusic.SetVolume(_backgroundMusicVolume, _backgroundMusicVolume); 
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
                    foreach (var s in _soundEffects) s.SetVolume(_effectsVolume, _effectsVolume);
            }
        }

        public string SoundPath { get; set; } = "Sounds";
        #endregion

        #region Constructors

        public DroidAudioManager()
        {
            // Initialize
            ActivateAudioSession();
        }

        #endregion

        #region Public Methods

        public void ActivateAudioSession()
        {
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
            if (_backgroundMusic != null)
            {
                //Stop and dispose of any background music
                _backgroundMusic.Stop();
                _backgroundMusic.Dispose();
            }

            _backgroundSong = filename;

            // Initialize background music
            _backgroundMusic = await NewSound(filename, BackgroundMusicVolume, true);

            return true;
        }

        public void StopBackgroundMusic()
        {
            // If any background music is playing, stop it
            _backgroundSong = "";
            if (_backgroundMusic != null)
            {
                _backgroundMusic.Stop();
                _backgroundMusic.Dispose();
            }
        }

        public void SuspendBackgroundMusic()
        {
            // If any background music is playing, stop it
            if (_backgroundMusic != null)
            {
                _backgroundMusic.Stop();
                _backgroundMusic.Dispose();
            }
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
            if (!MusicOn) return false;

            var effect = await NewSound(filename, EffectsVolume);
            _soundEffects.Add(effect);

            return true;
        }

        private async Task<MediaPlayer> NewSound(string filename, float defaultVolume, bool isLooping = false)
        {
            var fd = Forms.Context.Assets.OpenFd(Path.Combine(SoundPath, filename));

            // Initialize sound
            var soundEffect = new MediaPlayer();
            soundEffect.SetVolume(defaultVolume, defaultVolume);
            soundEffect.Completion += SoundEffectOnCompletion;

            soundEffect.Looping = isLooping;
            soundEffect.SetDataSource(fd.FileDescriptor);
            soundEffect.Prepare();
            soundEffect.Start();

            return soundEffect;
        }

        private void SoundEffectOnCompletion(object sender, EventArgs eventArgs)
        {
            var se = sender as MediaPlayer;

            if (se != _backgroundMusic)
                _soundEffects.Remove(se);

            se?.Dispose();
        }

        #endregion
    }
}