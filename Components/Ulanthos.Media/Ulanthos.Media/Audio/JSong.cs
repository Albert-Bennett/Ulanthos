using Ulanthos.Interfaces.Game;

namespace Ulanthos.Media.Audio
{
    public delegate void Ended(JSong song);

    /// <summary>
    /// Defines a piece of music.
    /// </summary>
    public class JSong : IAudio
    {
        JProp properties;
        JProp cached;
        Microsoft.DirectX.DirectSound.SecondaryBuffer music;

        int musicPosition;

        /// <summary>
        /// An event to determine when the Jsong has finished.
        /// </summary>
        public event Ended OnEnded;

        /// <summary>
        /// The current state of the JSong.
        /// </summary>
        public MediaStates CurrentState
        {
            get;
            private set;
        }

        /// <summary>
        /// The volume of the JSong.
        /// </summary>
        public int Volume 
        {
            get { return properties.Volume; }
            set { properties.Volume = value; }
        }

        /// <summary>
        /// Whether or not this is looped.
        /// </summary>
        public bool IsLooped { get; set; }

        /// <summary>
        /// Gets\sets the name of this JSong.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Whether or not this Jsong can be affected by an Jfx. 
        /// </summary>
        public bool JfxCompatible { get; set; }

        /// <summary>
        /// Creates a new JSong.
        /// </summary>
        /// <param name="filePath">The filepath for the JSong.</param>
        public JSong(string filePath, Microsoft.DirectX.DirectSound.Device audioDevice)
        {
            Microsoft.DirectX.DirectSound.BufferDescription desc = new Microsoft.DirectX.DirectSound.BufferDescription();

            if (JfxCompatible)
            {
                desc.ControlEffects = true;
                desc.ControlFrequency = true;
                desc.ControlPan = true;
                desc.ControlVolume = true;
                desc.Control3D = true;
            }
            else
            {
                desc.ControlEffects = false;
                desc.ControlVolume = true;
                desc.Control3D = true;
            }

            music = new Microsoft.DirectX.DirectSound.SecondaryBuffer(desc, audioDevice);
            music.Volume = properties.Volume;

            CurrentState = MediaStates.Playing;
        }

        /// <summary>
        /// Plays the JSong.
        /// </summary>
        public void Play()
        {  
            CurrentState = MediaStates.Playing;
            music.Play(1, Microsoft.DirectX.DirectSound.BufferPlayFlags.Default);
        }

        /// <summary>
        /// Resumes\ Pauses the JSong.
        /// </summary>
        public void PauseResume()
        {
            if (CurrentState == MediaStates.Playing)
            {
                CurrentState = MediaStates.Paused;

                int i = 0;
                music.GetCurrentPosition(out musicPosition, out i);
                music.Stop();
            }
            else if (CurrentState == MediaStates.Paused)
            {
                music.SetCurrentPosition(musicPosition);
                Play();
            }
        }

        /// <summary>
        /// Stops the JSong.
        /// </summary>
        public void Stop()
        {
            CurrentState = MediaStates.Stopped;
            music.Stop();
            OnEnded(this);
        }

        /// <summary>
        /// Applies a particular Jfx to this JSong.
        /// </summary>
        /// <param name="effect">The Jfx to be applied.</param>
        public void ApplyJfx(Jfx effect)
        {
            cached = properties;

            properties.Volume += effect.ChangeInVolume;
            properties.Pitch += effect.PitchChange;
            properties.Tempo += effect.TempoChange;
        }

        /// <summary>
        /// Sets the properties of the Jsong
        /// to the properties it previously had.
        /// </summary>
        public void Revert()
        {
            properties = cached;
        }

        /// <summary>
        /// Disposes of this JSong.
        /// </summary>
        public void Dispose()
        {
            if (music != null)
                music.Dispose();
        }
    }
}
