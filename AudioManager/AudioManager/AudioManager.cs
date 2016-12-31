using AudioManager.Interfaces;

namespace AudioManager
{
    public static class Audio
    {
        public static AudioManager Manager { get; } = new AudioManager();
    }

    public sealed class AudioManager : IAudioManager
    {
        #region Private Variables

        private readonly IAudioManager _soundProvider;

        #endregion

        #region Computed Properties

        public float BackgroundMusicVolume
        {
            get { return _soundProvider.BackgroundMusicVolume; }
            set { _soundProvider.BackgroundMusicVolume = value; }
        }

        public bool MusicOn
        {
            get { return _soundProvider.MusicOn; }
            set { _soundProvider.MusicOn = value; }
        }

        public float MusicVolume
        {
            get { return _soundProvider.MusicVolume; }
            set { _soundProvider.MusicVolume = value; }
        }

        public bool EffectsOn
        {
            get { return _soundProvider.EffectsOn; }
            set { _soundProvider.EffectsOn = value; }
        }

        public float EffectsVolume
        {
            get { return _soundProvider.EffectsVolume; }
            set { _soundProvider.EffectsVolume = value; }
        }

        public string SoundPath
        {
            get { return _soundProvider.SoundPath; }
            set { _soundProvider.SoundPath = value; }
        }

        #endregion

        #region Public Methods

        public void ActivateAudioSession()
        {
            _soundProvider.ActivateAudioSession();
        }

        public void DeactivateAudioSession()
        {
            _soundProvider.DeactivateAudioSession();
        }

        public void ReactivateAudioSession()
        {
            _soundProvider.ReactivateAudioSession();
        }

        public void PlayBackgroundMusic(string filename)
        {
            _soundProvider.PlayBackgroundMusic(filename);
        }

        public void StopBackgroundMusic()
        {
            _soundProvider.StopBackgroundMusic();
        }

        public void SuspendBackgroundMusic()
        {
            _soundProvider.SuspendBackgroundMusic();
        }

        public void RestartBackgroundMusic()
        {
            _soundProvider.RestartBackgroundMusic();
        }

        public void PlaySound(string filename)
        {
            _soundProvider.PlaySound(filename);
        }

        #endregion
    }
}
                                                                                                                            