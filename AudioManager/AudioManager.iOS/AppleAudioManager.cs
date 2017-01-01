using System.IO;
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

        private AVAudioPlayer _backgroundMusic;
        private AVAudioPlayer _soundEffect;
        private string _backgroundSong = "";

        #endregion

        #region Computed Properties

        public float BackgroundMusicVolume
        {
            get { return _backgroundMusic.Volume; }
            set { _backgroundMusic.Volume = value; }
        }

        public bool MusicOn { get; set; } = true;
        public float MusicVolume { get; set; } = 0.5f;

        public bool EffectsOn { get; set; } = true;
        public float EffectsVolume { get; set; } = 1.0f;

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

        public void PlayBackgroundMusic(string filename)
        {
            // Music enabled?
            if (!MusicOn) return;

            // Any existing background music?
            if (_backgroundMusic != null)
            {
                //Stop and dispose of any background music
                _backgroundMusic.Stop();
                _backgroundMusic.Dispose();
            }

            // Initialize background music
            var songUrl = new NSUrl(Path.Combine(SoundPath, filename));
            NSError err;
            _backgroundMusic = new AVAudioPlayer(songUrl, "mp3", out err);
            _backgroundMusic.Volume = MusicVolume;
            _backgroundMusic.FinishedPlaying += delegate {
                // backgroundMusic.Dispose(); 
                _backgroundMusic = null;
            };
            _backgroundMusic.NumberOfLoops = -1;
            _backgroundMusic.Play();
            _backgroundSong = filename;

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

        public void RestartBackgroundMusic()
        {
            // Music enabled?
            if (!MusicOn) return;

            // Was a song previously playing?
            if (_backgroundSong == "") return;

            // Restart song to fix issue with wonky music after sleep
            PlayBackgroundMusic(_backgroundSong);
        }

        public void PlaySound(string filename)
        {
            NSUrl songURL;

            // Music enabled?
            if (!EffectsOn) return;

            // Any existing sound effect?
            if (_soundEffect != null)
            {
                //Stop and dispose of any sound effect
                _soundEffect.Stop();
                _soundEffect.Dispose();
            }

            // Initialize sound
            songURL = new NSUrl(Path.Combine(SoundPath, filename));
            NSError err;
            _soundEffect = new AVAudioPlayer(songURL, "mp3", out err);
            _soundEffect.Volume = EffectsVolume;
            _soundEffect.FinishedPlaying += delegate {
                _soundEffect = null;
            };
            _soundEffect.NumberOfLoops = 0;
            _soundEffect.Play();

        }

        #endregion
    }
}