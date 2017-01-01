using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AudioManager.iOS;
using AudioManager.Interfaces;
using AVFoundation;
using Foundation;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppleAudioManager))]
namespace AudioManager.iOS
{
    public class AppleAudioManager : IAudioManager
    {
        #region Private Variables

        private readonly List<AVAudioPlayer> _soundEffects = new List<AVAudioPlayer>();

        private AVAudioPlayer _backgroundMusic;
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

        public AppleAudioManager()
        {
            // Initialize
            ActivateAudioSession();
        }

        #endregion

        #region Public Methods

        public void ActivateAudioSession()
        {
            // Initialize Audio
            var session = AVAudioSession.SharedInstance();
            session.SetCategory(AVAudioSessionCategory.Ambient);
            session.SetActive(true);
        }

        public void DeactivateAudioSession()
        {
            var session = AVAudioSession.SharedInstance();
            session.SetActive(false);
        }

        public void ReactivateAudioSession()
        {
            var session = AVAudioSession.SharedInstance();
            session.SetActive(true);
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
            if (!EffectsOn) return false;

            // Initialize sound
            var effect = await NewSound(filename, EffectsVolume);
            _soundEffects.Add(effect);

            return true;
        }

        private async Task<AVAudioPlayer> NewSound(string filename, float defaultVolume, bool isLooping = false)
        {

            var songUrl = new NSUrl(Path.Combine(SoundPath, filename));
            NSError err;
            var fileType = filename.Split('.').Last();
            var sound = new AVAudioPlayer(songUrl, fileType, out err)
            {
                Volume = defaultVolume,
                NumberOfLoops = isLooping ? -1 : 0
            };

            sound.FinishedPlaying += SoundOnFinishedPlaying;
                

            sound.Play();

            return sound;
        }

        private void SoundOnFinishedPlaying(object sender, AVStatusEventArgs avStatusEventArgs)
        {
            var se = sender as AVAudioPlayer;

            if (se != _backgroundMusic)
                _soundEffects.Remove(se);

            se?.Dispose();
        }

        #endregion
        }
}