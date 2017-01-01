using Xamarin.Forms;

namespace AudioManager.Interfaces
{
    public interface IAudioManager
    {
        #region Computed Properties

        float BackgroundMusicVolume { get; set; }

        bool MusicOn { get; set; }

        float MusicVolume { get; set; }

        bool EffectsOn { get; set; }

        float EffectsVolume { get; set; }

        string SoundPath { get; set; }

        #endregion


        #region Public Methods

        void ActivateAudioSession();

        void DeactivateAudioSession();

        void ReactivateAudioSession();

        void PlayBackgroundMusic(string filename);

        void StopBackgroundMusic();

        void SuspendBackgroundMusic();

        void RestartBackgroundMusic();

        void PlaySound(string filename);

        #endregion
    }
}
