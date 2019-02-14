using Ulanthos.Interfaces;

namespace Ulanthos.Media
{
    public delegate void Ending();

    /// <summary>
    /// Defines a video.
    /// </summary>
    public class Video : IName
    {
        Microsoft.DirectX.AudioVideoPlayback.Video vid;

        double currentPos;

        /// <summary>
        /// Returns the name of the Video.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Whether or not the Video is being played in full screen.
        /// </summary>
        public bool FullScreen
        {
            get
            {
                return vid.Fullscreen;
            }
            set
            {
                vid.Fullscreen = value;
            }
        }

        /// <summary>
        /// Returns the texture from the current frame of the Video.
        /// </summary>
        public Microsoft.DirectX.Direct3D.Texture CurrentTexture
        {
            get;
            private set;
        }

        /// <summary>
        /// The current state that the Video is in.
        /// </summary>
        public MediaStates CurrentState { get; private set; }

        /// <summary>
        /// An event to be thrown when the video ends.
        /// </summary>
        public event Ending Ending;

        /// <summary>
        /// Creates a new Video.
        /// </summary>
        /// <param name="filename">The filename for the Video.</param>
        public Video(string filename)
        {
            vid = new Microsoft.DirectX.AudioVideoPlayback.Video(filename, false);

            CurrentState = MediaStates.Stopped;
            vid.Ending += new System.EventHandler(OnEnding);
            vid.TextureReadyToRender += new Microsoft.DirectX.AudioVideoPlayback.TextureRenderEventHandler(OnTextureReady);
        }

        void OnTextureReady(object sender, Microsoft.DirectX.AudioVideoPlayback.TextureRenderEventArgs e)
        {
            CurrentTexture = e.Texture;
        }

        void OnEnding(object sender, System.EventArgs e)
        {
            CurrentState = MediaStates.Stopped;

            if (Ending != null)
                Ending();
        }

        /// <summary>
        /// Plays the Video.
        /// </summary>
        public void Play()
        {
            CurrentState = MediaStates.Playing;
            vid.Play();
        }

        /// <summary>
        /// Pauses or resumes the Video.
        /// </summary>
        public void PauseResume()
        {
            if (CurrentState == MediaStates.Playing)
            {
                CurrentState = MediaStates.Paused;
                currentPos = vid.CurrentPosition;
                vid.Pause();
            }
            else if (CurrentState == MediaStates.Paused)
            {
                CurrentState = MediaStates.Playing;
                vid.CurrentPosition = currentPos;
                vid.Play();
            }
        }

        /// <summary>
        /// Stops the Video.
        /// </summary>
        public void Stop()
        {
            if (Ending != null)
            {
                Ending();
                CurrentState = MediaStates.Stopped;
            }
        }

        /// <summary>
        /// Disposes of the Video.
        /// </summary>
        public void Dispose()
        {
            vid.Dispose();
        }
    }
}
