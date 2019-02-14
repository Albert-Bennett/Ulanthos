using Ulanthos.Interfaces;

namespace Ulanthos.Media.Audio
{
    /// <summary>
    /// Used to define a piece of audio.
    /// </summary>
    public interface IAudio : IName, IDispose
    {
        bool JfxCompatible { get; set; }
        int Volume { get; set; }

        void Play();
        void PauseResume();
        void Stop();
        void ApplyJfx(Jfx effect);
    }
}
