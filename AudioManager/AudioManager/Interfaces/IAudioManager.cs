using System.Threading.Tasks;
using Xamarin.Forms;

namespace AudioManager.Interfaces
{
    public interface IAudioManager
    {
        #region Computed Properties

        float BackgroundMusicVolume { get; set; }

        bool MusicOn { get; set; }

        bool EffectsOn { get; set; }

        float EffectsVolume { get; set; }

        string SoundPath { get; set; }

        #endregion


        #region Public Methods

        void ActivateAudioSession();

        void DeactivateAudioSession();

        void ReactivateAudioSession();

        Task<bool> PlayBackgroundMusic(string filename);

        void StopBackgroundMusic();

        void SuspendBackgroundMusic();

        Task<bool> RestartBackgroundMusic();

        Task<bool> PlaySound(string filename);

        #endregion
    }
}
