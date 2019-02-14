using System.Collections.Generic;
using Ulanthos.Interfaces;

namespace Ulanthos.Media.Audio
{
    public delegate void VolumeChanged(JPla playlist);

    /// <summary>
    /// Defines a playlist.
    /// </summary>
    public class JPla : IName
    {
        int volume;

        /// <summary>
        /// An event to be thrown when the volume has been changed.
        /// </summary>
        public event VolumeChanged OnVolumeChanged;

        /// <summary>
        /// Whether or not the global volume should be used.
        /// </summary>
        public bool UseGlobalVolume { get; set; }

        /// <summary>
        /// Global volume.
        /// </summary>
        public int GlobalVolume
        {
            get { return volume; }
            set
            {
                if (OnVolumeChanged != null)
                    OnVolumeChanged(this);

                volume = value;
            }
        }

        /// <summary>
        /// The name of the JPla.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A reference list of all of the music in the JPla.
        /// </summary>
        public Dictionary<string, string> Music { get; set; }
    }
}
