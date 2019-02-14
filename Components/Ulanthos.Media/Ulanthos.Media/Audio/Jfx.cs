namespace Ulanthos.Media.Audio
{
    /// <summary>
    /// Defines an audio effect.
    /// </summary>
    public class Jfx
    {
        /// <summary>
        /// The change in the volume of the audio.
        /// </summary>
        public int ChangeInVolume { get; set; }

        /// <summary>
        /// The change in the pitch in the audio.
        /// </summary>
        public float PitchChange { get; set; }

        /// <summary>
        /// The change of tempo in the audio.
        /// </summary>
        public float TempoChange { get; set; }

        /// <summary>
        /// The amoun of echo to be applied.
        /// </summary>
        public float Echo { get; set; }
    }
}
