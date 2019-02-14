using Microsoft.DirectX.DirectInput;

namespace Ulanthos.Framework.Input
{
    /// <summary>
    /// Defines the Keyboard input device.
    /// </summary>
    public class Keyboard : UlanComponent
    {
        Device keyBoard;
        KeyboardState current;
        KeyboardState previous;
        KeyboardState empty;

        /// <summary>
        /// Creates a new Keyboard.
        /// </summary>
        public Keyboard(Game game)
            : base("Keyboard")
        {
            keyBoard = new Device(SystemGuid.Keyboard);
            keyBoard.SetCooperativeLevel(game, CooperativeLevelFlags.Background | CooperativeLevelFlags.NonExclusive);
            keyBoard.Acquire();

            empty = keyBoard.GetCurrentKeyboardState();
            Reset();
        }

        public override void Update()
        {
            previous = current;
            current = keyBoard.GetCurrentKeyboardState();
        }

        /// <summary>
        /// Is a spasific key pressed.
        /// </summary>
        /// <param name="key">The key to be checked.</param>
        /// <returns>The result of the check.</returns>
        public bool IsKeyPressed(Key key)
        {
            return current[key];
        }

        /// <summary>
        /// Is a spasific key being held down.
        /// </summary>
        /// <param name="key">The key to be checked.</param>
        /// <returns>The result of the check.</returns>
        public bool IsKeyHeld(Key key)
        {
            return previous[key] && current[key];
        }

        /// <summary>
        /// Has a spasific key been released.
        /// </summary>
        /// <param name="key">The key to be checked.</param>
        /// <returns>The result of the check.</returns>
        public bool IsKeyReleased(Key key)
        {
            return !current[key];
        }

        /// <summary>
        /// Was a spacific key stroked.
        /// </summary>
        /// <param name="key">The key to be checked.</param>
        /// <returns>The result of the check.</returns>
        public bool IsKeyStroked(Key key)
        {
            return current[key] && !previous[key];
        }

        /// <summary>
        /// Resets the Keyboard.
        /// </summary>
        public void Reset()
        {
            current = previous = empty;
        }
    }
}
