using System.Windows.Forms;
using Microsoft.DirectX.Direct3D;
using Ulanthos.Framework.Input;
using Ulanthos.Math;

namespace Ulanthos.Framework
{
    public partial class Game : Form
    {
        /// <summary>
        /// The graphics device for the game.
        /// </summary>
        public static GraphicsDevice Device { get; private set; }
        ComponentManager ulanComponents;

        /// <summary>
        /// Returns a keyboard input device.
        /// </summary>
        public static Keyboard Keyboard { get; private set; }

        /// <summary>
        /// Returns a mouse input device.
        /// </summary>
        public static Mouse Mouse { get; private set; }

        AudioManager audio;

        /// <summary>
        /// Creates a new Game.
        /// </summary>
        public Game()
        {
            ClientSize = new System.Drawing.Size(640, 480);
            Text = "Ulanthos V1";
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
        }

        /// <summary>
        /// Initializes the Game.
        /// </summary>
        protected virtual void Initialize()
        {
            ulanComponents = new ComponentManager();

            Device = new GraphicsDevice(this);
            audio = new AudioManager(this);

            ulanComponents.Initialize();

            Load();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.Update();
            this.Render();

            this.Invalidate();
        }

        /// <summary>
        /// Updates the Game. 
        /// </summary>
        protected virtual void Update()
        {
            ulanComponents.Update();

#if Debug
            if(Keyboard.IsKeyStroked(Microsoft.DirectX.DirectInput.Key.Escape))
                Exit();
#endif
        }

        /// <summary>
        /// Loads content aspects of the Game.
        /// </summary>
        protected virtual void Load()
        {
            ulanComponents.Load();
        }

        /// <summary>
        /// Renders the Game.
        /// </summary>
        protected virtual void Render()
        {
            ulanComponents.Render();
        }

        /// <summary>
        /// Disposes of the Game.
        /// </summary>
        /// <param name="disposing">Disposing.</param>
        protected override void Dispose(bool disposing)
        {
            ulanComponents.Unload();
            ulanComponents.Dispose();
            ulanComponents = null;

            Device.Dispose();
            this.Dispose();

            base.Dispose();
        }

        #region Helpers

        /// <summary>
        /// Changes the client size.
        /// </summary>
        /// <param name="size">The new size of the client.</param>
        protected void ChangeClientSize(Size size)
        {
            ClientSize = new System.Drawing.Size((int)size.Width, (int)size.Height);
        }

        /// <summary>
        /// Runs the game.
        /// </summary>
        public void Run()
        {
            Show();
            Initialize();
            Application.Run(this);
        }

        /// <summary>
        /// Exits the Game.
        /// </summary>
        public void Exit()
        {
            Dispose(true);
        }

        #endregion
    }
}
