using Microsoft.DirectX.DirectInput;
using Ulanthos.Math;

namespace Ulanthos.Framework.Input
{
    /// <summary>
    /// The different buttons on a mouse.
    /// </summary>
    public enum MouseButtons
    {
        Left = 0,
        Right = 1,
        Middle = 2
    }

    /// <summary>
    /// Defines the mouse input device.
    /// </summary>
    public class Mouse : UlanComponent
    {
        Device mouse;
        MouseState current;
        MouseState previous;
        MouseState empty;

        /// <summary>
        /// Creates a new Mouse.
        /// </summary>
        /// <param name="game">Game.</param>
        public Mouse(Game game)
            : base("Mouse")
        {
            mouse = new Device(SystemGuid.Mouse);
            mouse.SetCooperativeLevel(game, CooperativeLevelFlags.Background | CooperativeLevelFlags.NonExclusive);
            mouse.Acquire();

            empty = mouse.CurrentMouseState;
            Reset();
        }

        /// <summary>
        /// Returns the position of the mouse.
        /// </summary>
        public Vector2 MousePos
        {
            get { return new Vector2(current.X, current.Y); }
        }

        public override void Update()
        {
            previous = current;
            current = mouse.CurrentMouseState;
        }

        /// <summary>
        /// A check to see which mouse button has been pressed.
        /// </summary>
        /// <param name="button">The button to be checked.</param>
        /// <returns>The result of the check.</returns>
        public bool IsPressed(MouseButtons button)
        {
            byte[] pressed = current.GetMouseButtons();

            return pressed[(int)button] == 0;
        }

        /// <summary>
        /// A check to see if a mouse button has been held down.
        /// </summary>
        /// <param name="button">The button to be checked.</param>
        /// <returns>The result of the check.</returns>
        public bool IsHeld(MouseButtons button)
        {
            byte[] curr = current.GetMouseButtons();
            byte[] prev = previous.GetMouseButtons();

            return curr[(int)button] == 0 && prev[(int)button] == 0;
        }

        /// <summary>
        /// A check to see which mouse button has been released.
        /// </summary>
        /// <param name="button">The button to be checked.</param>
        /// <returns>The result of the check.</returns>
        public bool IsReleased(MouseButtons button)
        {
            byte[] curr = current.GetMouseButtons();

            return curr[(int)button] == 1;
        }

        /// <summary>
        /// A check to see if a mouse button has been stroked.
        /// </summary>
        /// <param name="button">The button to be checked.</param>
        /// <returns>The result of the check.</returns>
        public bool IsStroked(MouseButtons button)
        {
            byte[] curr = current.GetMouseButtons();
            byte[] prev = previous.GetMouseButtons();

            return curr[(int)button] == 0 && prev[(int)button] == 1;
        }

        /// <summary>
        /// Resets the mouse.
        /// </summary>
        public void Reset()
        {
            current = previous = empty;
        }
    }
}
