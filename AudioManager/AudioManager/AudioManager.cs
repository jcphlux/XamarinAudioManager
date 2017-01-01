using AudioManager.Interfaces;
using Xamarin.Forms;

namespace AudioManager
{
    public static class Audio
    {
        public static IAudioManager Manager { get; } = DependencyService.Get<IAudioManager>();

        
    }
}
                                                                                                                            