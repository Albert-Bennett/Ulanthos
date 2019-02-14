using Ulanthos.Framework;

namespace Ulanthos
{
    /// <summary>
    /// The main entry point of the Game.
    /// </summary>
    class Program
    {
        static void Main()
        {
            using (Game game = new Game())
            {
                game.Run();
            }
        }
    }
}
