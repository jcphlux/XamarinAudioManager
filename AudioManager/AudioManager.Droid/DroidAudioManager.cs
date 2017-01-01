using System.IO;
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

        private MediaPlayer _backgroundMusic;
        private MediaPlayer _soundEffect;
        private string _backgroundSong = "";
        private float _backgroundMusicVolume;

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
                _backgroundMusic.SetVolume(value, value);
                _backgroundMusicVolume = value;
            }
        }

        public bool MusicOn { get; set; } = true;
        public float MusicVolume { get; set; } = 0.5f;

        public bool EffectsOn { get; set; } = true;
        public float EffectsVolume { get; set; } = 1.0f;

        public string SoundPath { get; set; } = "Sounds";
        #endregion

        #region Constructors

        public DroidAudioManager()
        {
            // Initialize
            ActivateAudioSession();
        }

        #endregion

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
            // Open the resource
            var fd = Forms.Context.Assets.OpenFd($"{SoundPath}/{filename}");

            _backgroundMusic = new MediaPlayer();
            BackgroundMusicVolume = MusicVolume;
            _backgroundMusic.Looping = true;

            _backgroundMusic.SetDataSource(fd.FileDescriptor);
            _backgroundMusic.Prepare();
            _backgroundMusic.Start();
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
            // Music enabled?
            if (!MusicOn) return;

            // Any existing background music?
            if (_soundEffect != null)
            {
                //Stop and dispose of any background music
                _soundEffect.Stop();
                _soundEffect.Dispose();
            }

            // Initialize background music
            // Open the resource
            var fd = Forms.Context.Assets.OpenFd(Path.Combine(SoundPath, filename));

            _soundEffect = new MediaPlayer();
            _soundEffect.SetVolume(EffectsVolume, EffectsVolume);
            _soundEffect.Completion += delegate
            {
                _soundEffect.Dispose();
                _soundEffect = null;
            };
            _soundEffect.Looping = false;
            _soundEffect.SetDataSource(fd.FileDescriptor);
            _soundEffect.Prepare();
            _soundEffect.Start();
        }
    }
}